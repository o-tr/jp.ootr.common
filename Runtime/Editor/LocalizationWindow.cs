#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using jp.ootr.common.Localization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.ootr.common.Editor
{
    public class LocalizationWindow : EditorWindow
    {
        [SerializeField] private StyleSheet baseStyleSheet;

        private BaseClass _target;
        private ObjectField _targetField;
        private VisualElement _tableContainer;
        private List<string> _logicalKeys = new List<string>();
        private Dictionary<string, Dictionary<Localization.Language, string>> _keyToLangToValue =
            new Dictionary<string, Dictionary<Localization.Language, string>>();

        private static readonly Localization.Language[] AllLanguages =
            (Localization.Language[])Enum.GetValues(typeof(Localization.Language));

        private const float DefaultKeyColumnWidth = 220f;
        private const float DefaultLangColumnWidth = 160f;
        private const float MinColumnWidth = 60f;
        private const float MaxColumnWidth = 500f;
        private const float ResizerWidth = 7f;
        private const float ResizerOffset = ResizerWidth / 2f + 1f;

        [SerializeField] private float _keyColumnWidth = DefaultKeyColumnWidth;
        [SerializeField] private List<float> _langColumnWidths = new List<float>(); // index = (int)Language
        [SerializeField] private List<Localization.Language> _explicitlyAddedLanguages = new List<Localization.Language>();
        private HashSet<Localization.Language> _loadedLanguages = new HashSet<Localization.Language>();

        private DropdownField _langDropdown;

        private List<List<VisualElement>> _columnCellRefs = new List<List<VisualElement>>();
        private List<float> _columnWidths = new List<float>();
        private List<Localization.Language> _visibleLangsSnapshot = new List<Localization.Language>();
        private int _resizingColumnIndex = -1;
        private float _resizeStartX;
        private float _resizeStartWidth;

        private static Color HeaderBackground => EditorGUIUtility.isProSkin
            ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.76f, 0.76f, 0.76f);
        private static Color HeaderBorder => EditorGUIUtility.isProSkin
            ? new Color(0.25f, 0.25f, 0.25f) : new Color(0.60f, 0.60f, 0.60f);
        private static Color CellBorder => EditorGUIUtility.isProSkin
            ? new Color(0.22f, 0.22f, 0.22f) : new Color(0.70f, 0.70f, 0.70f);
        private static Color RowEven => EditorGUIUtility.isProSkin
            ? new Color(0.16f, 0.16f, 0.16f) : new Color(0.83f, 0.83f, 0.83f);
        private static Color RowOdd => EditorGUIUtility.isProSkin
            ? new Color(0.14f, 0.14f, 0.14f) : new Color(0.78f, 0.78f, 0.78f);

        public void CreateGUI()
        {
            var root = new VisualElement();
            if (baseStyleSheet != null)
            {
                root.styleSheets.Add(baseStyleSheet);
                root.AddToClassList("root");
            }

            root.Add(GetTargetPicker());
            var toolbar = new VisualElement { style = { flexDirection = FlexDirection.Row, marginBottom = 4 } };
            var addKeyBtn = new Button(OnAddKey) { text = "Add Key" };
            var saveBtn = new Button(OnSave) { text = "Save" };
            _langDropdown = new DropdownField { style = { minWidth = 120 } };
            UpdateLangDropdownChoices();
            var addLangBtn = new Button(OnAddLanguage) { text = "Add" };
            toolbar.Add(addKeyBtn);
            toolbar.Add(saveBtn);
            toolbar.Add(_langDropdown);
            toolbar.Add(addLangBtn);
            root.Add(toolbar);

            _tableContainer = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            _tableContainer.style.flexGrow = 1;
            _tableContainer.style.minHeight = 200;
            root.Add(_tableContainer);

            if (_target != null)
                ReloadTable();

            rootVisualElement.Add(root);
        }

        [MenuItem("Tools/ootr/Localization Window")]
        public static void ShowWindow()
        {
            GetWindow<LocalizationWindow>().titleContent = new GUIContent("Localization");
        }

        public static void ShowWindowWithTarget(BaseClass target)
        {
            var wnd = GetWindow<LocalizationWindow>();
            wnd.titleContent = new GUIContent("Localization");
            wnd._target = target;
            if (wnd._targetField != null)
                wnd._targetField.SetValueWithoutNotify(target);
            if (wnd._tableContainer != null)
                wnd.ReloadTable();
        }

        private VisualElement GetTargetPicker()
        {
            var root = new VisualElement();
            _targetField = new ObjectField
            {
                label = "Target",
                objectType = typeof(BaseClass),
                value = _target
            };
            _targetField.RegisterValueChangedCallback(evt =>
            {
                _target = (BaseClass)evt.newValue;
                ReloadTable();
            });
            root.Add(_targetField);
            return root;
        }

        private void LoadFromTarget()
        {
            if (_target == null)
            {
                _logicalKeys.Clear();
                _keyToLangToValue.Clear();
                _explicitlyAddedLanguages.Clear();
                _loadedLanguages.Clear();
                return;
            }

            var so = new SerializedObject(_target);
            var keysProp = so.FindProperty("localizationKeys");
            var valuesProp = so.FindProperty("localizationValues");

            _logicalKeys.Clear();
            _keyToLangToValue.Clear();
            _explicitlyAddedLanguages.Clear();
            _loadedLanguages.Clear();

            if (keysProp == null || valuesProp == null || keysProp.arraySize != valuesProp.arraySize)
                return;

            var keyOrder = new List<string>();
            var seen = new HashSet<string>();

            for (var i = 0; i < keysProp.arraySize; i++)
            {
                var fullKey = keysProp.GetArrayElementAtIndex(i).stringValue;
                var value = valuesProp.GetArrayElementAtIndex(i).stringValue ?? "";
                if (string.IsNullOrEmpty(fullKey)) continue;

                var dot = fullKey.IndexOf('.');
                if (dot < 0) continue;
                var langStr = fullKey.Substring(0, dot);
                var logicalKey = fullKey.Substring(dot + 1);
                var lang = LanguageUtils.FromStr(langStr);
                if (lang == null) continue;
                _loadedLanguages.Add(lang.Value);

                if (!_keyToLangToValue.TryGetValue(logicalKey, out var dict))
                {
                    _keyToLangToValue[logicalKey] = dict = new Dictionary<Localization.Language, string>();
                    if (!seen.Contains(logicalKey))
                    {
                        seen.Add(logicalKey);
                        keyOrder.Add(logicalKey);
                    }
                }

                dict[lang.Value] = value;
            }

            _logicalKeys = keyOrder;
        }

        /// <summary>
        /// Returns languages that have at least one non-empty value in the current data.
        /// Order follows AllLanguages (enum order).
        /// </summary>
        private List<Localization.Language> GetVisibleLanguages()
        {
            var hasValue = new HashSet<Localization.Language>();
            foreach (var dict in _keyToLangToValue.Values)
            {
                foreach (var kv in dict)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                        hasValue.Add(kv.Key);
                }
            }

            // 明示的に追加された言語も含める
            foreach (var lang in _explicitlyAddedLanguages)
                hasValue.Add(lang);

            // いずれのキーにも値がない場合は全言語を表示（新規入力用）
            if (hasValue.Count == 0 && _logicalKeys.Count > 0)
                return new List<Localization.Language>(AllLanguages);
            return AllLanguages.Where(hasValue.Contains).ToList();
        }

        private void UpdateLangDropdownChoices()
        {
            if (_langDropdown == null) return;
            var current = new HashSet<Localization.Language>(GetVisibleLanguages());
            var choices = AllLanguages
                .Where(l => !current.Contains(l))
                .Select(l => l.ToString())
                .ToList();
            _langDropdown.choices = choices;
            _langDropdown.value = choices.Count > 0 ? choices[0] : "";
        }

        private void OnAddLanguage()
        {
            if (_target == null || _langDropdown == null) return;
            if (string.IsNullOrEmpty(_langDropdown.value)) return;
            if (!Enum.TryParse<Localization.Language>(_langDropdown.value, out var lang)) return;
            if (!_explicitlyAddedLanguages.Contains(lang))
                _explicitlyAddedLanguages.Add(lang);
            ReloadTable(loadFromTarget: false);
        }

        private float GetLangColumnWidth(Localization.Language lang)
        {
            var idx = (int)lang;
            while (_langColumnWidths.Count <= idx)
                _langColumnWidths.Add(DefaultLangColumnWidth);
            return Mathf.Clamp(_langColumnWidths[idx], MinColumnWidth, MaxColumnWidth);
        }

        private void SetLangColumnWidth(Localization.Language lang, float width)
        {
            var idx = (int)lang;
            while (_langColumnWidths.Count <= idx)
                _langColumnWidths.Add(DefaultLangColumnWidth);
            _langColumnWidths[idx] = Mathf.Clamp(width, MinColumnWidth, MaxColumnWidth);
        }

        private static void SetCellStyle(VisualElement cell, float width)
        {
            cell.style.width = width;
            cell.style.minWidth = width;
            cell.style.maxWidth = width;
            cell.style.flexShrink = 0;
            cell.style.flexGrow = 0;
            cell.style.borderRightWidth = 1;
            cell.style.borderRightColor = new StyleColor(CellBorder);
            cell.style.marginLeft = 0;
            cell.style.marginRight = 0;
            cell.style.marginTop = 0;
            cell.style.marginBottom = 0;
            cell.style.paddingLeft = 4;
            cell.style.paddingRight = 4;
        }

        private VisualElement CreateResizer(int columnIndex)
        {
            var resizer = new VisualElement();
            resizer.style.width = ResizerWidth;
            resizer.style.minWidth = ResizerWidth;
            resizer.style.right = -ResizerOffset;
            resizer.style.flexShrink = 0;
            resizer.style.backgroundColor = new StyleColor(HeaderBorder);

            resizer.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0) return;
                _resizingColumnIndex = columnIndex;
                _resizeStartX = evt.mousePosition.x;
                _resizeStartWidth = _columnWidths[columnIndex];
                resizer.CaptureMouse();
                evt.StopPropagation();
            });

            resizer.RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (_resizingColumnIndex != columnIndex) return;
                var delta = evt.mousePosition.x - _resizeStartX;
                var newWidth = Mathf.Clamp(_resizeStartWidth + delta, MinColumnWidth, MaxColumnWidth);
                ApplyColumnWidth(columnIndex, newWidth);
                evt.StopPropagation();
            });

            resizer.RegisterCallback<MouseUpEvent>(evt =>
            {
                if (evt.button != 0) return;
                if (_resizingColumnIndex == columnIndex)
                {
                    resizer.ReleaseMouse();
                    _resizingColumnIndex = -1;
                }
                evt.StopPropagation();
            });

            return resizer;
        }

        private VisualElement CreateHeaderCell(string title, int columnIndex)
        {
            var container = new VisualElement();
            container.style.position = Position.Relative;
            SetCellStyle(container, _columnWidths[columnIndex]);

            var label = new Label(title);
            label.AddToClassList("unity-font-element-bold");
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            container.Add(label);

            var resizer = CreateResizer(columnIndex);
            resizer.style.position = Position.Absolute;
            resizer.style.top = 0;
            resizer.style.bottom = 0;
            container.Add(resizer);

            _columnCellRefs[columnIndex].Add(container);
            return container;
        }

        private void ApplyColumnWidth(int columnIndex, float width)
        {
            _columnWidths[columnIndex] = width;
            if (columnIndex == 0)
                _keyColumnWidth = width;
            else if (columnIndex <= _visibleLangsSnapshot.Count)
                SetLangColumnWidth(_visibleLangsSnapshot[columnIndex - 1], width);

            if (columnIndex >= _columnCellRefs.Count) return;
            var cells = _columnCellRefs[columnIndex];
            if (cells == null) return;
            foreach (var c in cells)
            {
                c.style.width = width;
                c.style.minWidth = width;
                c.style.maxWidth = width;
            }
        }

        /// <param name="loadFromTarget">true のとき SerializedObject から再読み込み。Add Key 時は false でメモリ上のデータのみでテーブル再描画。</param>
        private void ReloadTable(bool loadFromTarget = true)
        {
            if (loadFromTarget)
                LoadFromTarget();
            _tableContainer.Clear();
            _columnCellRefs.Clear();
            _columnWidths.Clear();
            _visibleLangsSnapshot = GetVisibleLanguages();

            _keyColumnWidth = Mathf.Clamp(_keyColumnWidth, MinColumnWidth, MaxColumnWidth);
            _columnWidths.Add(_keyColumnWidth);
            for (var i = 0; i < _visibleLangsSnapshot.Count; i++)
                _columnWidths.Add(GetLangColumnWidth(_visibleLangsSnapshot[i]));

            var table = new VisualElement();
            table.style.flexDirection = FlexDirection.Column;
            table.style.flexShrink = 0;
            var totalWidth = _columnWidths.Sum();
            table.style.minWidth = totalWidth;

            var keyColumnCells = new List<VisualElement>();
            _columnCellRefs.Add(keyColumnCells);
            for (var i = 0; i < _visibleLangsSnapshot.Count; i++)
                _columnCellRefs.Add(new List<VisualElement>());

            // Header row
            var headerRow = new VisualElement { style = { flexDirection = FlexDirection.Row, flexShrink = 0 } };
            headerRow.style.marginLeft = 0;
            headerRow.style.marginRight = 0;
            headerRow.style.marginTop = 0;
            headerRow.style.marginBottom = 0;
            headerRow.style.backgroundColor = new StyleColor(HeaderBackground);
            headerRow.style.borderBottomWidth = 1;
            headerRow.style.borderBottomColor = new StyleColor(HeaderBorder);

            var keyHeaderCell = CreateHeaderCell("Key", 0);
            headerRow.Add(keyHeaderCell);

            for (var c = 0; c < _visibleLangsSnapshot.Count; c++)
            {
                var headerCell = CreateHeaderCell(_visibleLangsSnapshot[c].ToString(), c + 1);
                headerRow.Add(headerCell);
            }
            table.Add(headerRow);

            for (var rowIndex = 0; rowIndex < _logicalKeys.Count; rowIndex++)
            {
                var logicalKey = _logicalKeys[rowIndex];
                var langToValue = _keyToLangToValue[logicalKey];

                var row = new VisualElement { style = { flexDirection = FlexDirection.Row, flexShrink = 0 } };
                row.style.marginLeft = 0;
                row.style.marginRight = 0;
                row.style.marginTop = 0;
                row.style.marginBottom = 0;
                row.style.backgroundColor = new StyleColor(rowIndex % 2 == 0 ? RowEven : RowOdd);

                var keyField = new TextField { value = logicalKey };
                SetCellStyle(keyField, _columnWidths[0]);
                keyColumnCells.Add(keyField);
                var idx = rowIndex;
                keyField.RegisterValueChangedCallback(evt =>
                {
                    var newKey = evt.newValue?.Trim() ?? "";
                    if (string.IsNullOrEmpty(newKey) || newKey == _logicalKeys[idx]) return;
                    var oldKey = _logicalKeys[idx];
                    if (_keyToLangToValue.ContainsKey(newKey)) return;
                    _keyToLangToValue[newKey] = _keyToLangToValue[oldKey];
                    _keyToLangToValue.Remove(oldKey);
                    _logicalKeys[idx] = newKey;
                });
                row.Add(keyField);

                for (var c = 0; c < _visibleLangsSnapshot.Count; c++)
                {
                    var lang1 = _visibleLangsSnapshot[c];
                    var current = langToValue.TryGetValue(lang1, out var v) ? v : "";
                    var tf = new TextField { value = current };
                    SetCellStyle(tf, _columnWidths[c + 1]);
                    _columnCellRefs[c + 1].Add(tf);
                    tf.RegisterValueChangedCallback(evt =>
                    {
                        var key = _logicalKeys[idx];
                        if (!_keyToLangToValue.TryGetValue(key, out var d))
                        {
                            d = new Dictionary<Localization.Language, string>();
                            _keyToLangToValue[key] = d;
                        }
                        d[lang1] = evt.newValue;
                    });
                    row.Add(tf);
                }

                table.Add(row);
            }

            _tableContainer.Add(table);
            UpdateLangDropdownChoices();
        }

        private void OnAddKey()
        {
            if (_target == null) return;
            var baseName = "new_key";
            var name = baseName;
            var c = 0;
            while (_keyToLangToValue.ContainsKey(name))
                name = $"{baseName}_{++c}";
            _logicalKeys.Add(name);
            _keyToLangToValue[name] = new Dictionary<Localization.Language, string>();
            ReloadTable(loadFromTarget: false);
        }

        private void OnSave()
        {
            if (_target == null) return;

            var saveLangs = AllLanguages
                .Where(l => _loadedLanguages.Contains(l) || _explicitlyAddedLanguages.Contains(l))
                .ToList();
            var pairs = new List<(string fullKey, string value)>();
            foreach (var key in _logicalKeys)
            {
                var dict = _keyToLangToValue[key];
                foreach (var lang in saveLangs)
                {
                    var value = dict.TryGetValue(lang, out var v) ? v : "";
                    var fullKey = $"{LanguageUtils.ToStr(lang)}.{key}";
                    pairs.Add((fullKey, value));
                }
            }

            var so = new SerializedObject(_target);
            var keysProp = so.FindProperty("localizationKeys");
            var valuesProp = so.FindProperty("localizationValues");
            if (keysProp == null || valuesProp == null) return;

            keysProp.arraySize = pairs.Count;
            valuesProp.arraySize = pairs.Count;
            for (var i = 0; i < pairs.Count; i++)
            {
                keysProp.GetArrayElementAtIndex(i).stringValue = pairs[i].fullKey;
                valuesProp.GetArrayElementAtIndex(i).stringValue = pairs[i].value;
            }

            Undo.RecordObject(_target, "Edit Localization Data");
            so.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(_target);
        }
    }
}
#endif
