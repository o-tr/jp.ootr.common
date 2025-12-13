#if UNITY_EDITOR
using System.Linq;
using jp.ootr.common.Base;
using jp.ootr.common.ColorSchema;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jp.ootr.common.Editor
{
    [CustomEditor(typeof(ColorSchemaApplierBase), true)]
    public class ColorSchemaApplierEditor : UnityEditor.Editor
    {
        private static readonly Color BackgroundColor = new Color(0.22f, 0.22f, 0.22f);
        private static readonly Color BorderColor = new Color(0.4f, 0.4f, 0.4f);
        private static readonly Color HoverBackgroundColor = new Color(0.3f, 0.3f, 0.3f);
        private static readonly Color SelectedBackgroundColor = new Color(0.3f, 0.5f, 0.9f, 0.3f);
        private static readonly Color GrayColor = Color.gray;

        private static void SetBorder(IStyle style, float width, Color color)
        {
            style.borderLeftWidth = width;
            style.borderRightWidth = width;
            style.borderTopWidth = width;
            style.borderBottomWidth = width;
            style.borderLeftColor = new StyleColor(color);
            style.borderRightColor = new StyleColor(color);
            style.borderTopColor = new StyleColor(color);
            style.borderBottomColor = new StyleColor(color);
        }

        private SerializedProperty _schemaNameProperty;
        private BaseClass _baseClass;
        private string[] _schemaNames;
        private Color[] _schemaColors;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            _schemaNameProperty = serializedObject.FindProperty("schemaName");
            if (_schemaNameProperty == null)
            {
                root.Add(new Label("Error: schemaName property not found"));
                return root;
            }

            _baseClass = ColorSchemaUtils.FindNearestBaseClass((target as MonoBehaviour)?.transform);
            UpdateSchemaData();

            if (_baseClass != null && _schemaNames != null && _schemaNames.Length > 0)
            {
                root.Add(CreateDropdownField());
            }
            else
            {
                root.Add(CreateTextField());
            }

            return root;
        }


        private void UpdateSchemaData()
        {
            if (_baseClass == null)
            {
                _schemaNames = null;
                _schemaColors = null;
                return;
            }

            var so = new SerializedObject(_baseClass);
            so.Update();

            var namesProperty = so.FindProperty("colorSchemaNames");
            var colorsProperty = so.FindProperty("colorSchemas");

            if (namesProperty == null || colorsProperty == null ||
                namesProperty.arraySize == 0 || colorsProperty.arraySize == 0)
            {
                _schemaNames = null;
                _schemaColors = null;
                return;
            }

            var arraySize = Mathf.Min(namesProperty.arraySize, colorsProperty.arraySize);
            _schemaNames = new string[arraySize];
            _schemaColors = new Color[arraySize];

            for (var i = 0; i < arraySize; i++)
            {
                _schemaNames[i] = namesProperty.GetArrayElementAtIndex(i).stringValue;
                _schemaColors[i] = colorsProperty.GetArrayElementAtIndex(i).colorValue;
            }
        }

        private VisualElement CreateDropdownField()
        {
            var container = new VisualElement();

            var currentSchemaName = _schemaNameProperty.stringValue;
            var currentIndex = _schemaNames != null 
                ? System.Array.IndexOf(_schemaNames, currentSchemaName) 
                : -1;
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }

            var customDropdown = new ColorSchemaDropdownField(
                "Schema Name",
                _schemaNames,
                _schemaColors,
                currentIndex
            );

            customDropdown.RegisterValueChangedCallback(evt =>
            {
                var selectedSchemaName = evt.newValue;
                _schemaNameProperty.stringValue = selectedSchemaName;
                serializedObject.ApplyModifiedProperties();
                ApplyColorToComponent(selectedSchemaName);
            });

            container.Add(customDropdown);
            return container;
        }

        private class ColorSchemaDropdownField : VisualElement
        {
            private readonly string[] _schemaNames;
            private readonly Color[] _schemaColors;
            private int _selectedIndex;
            private VisualElement _selectedDisplay;
            private VisualElement _dropdownButton;

            public ColorSchemaDropdownField(string label, string[] schemaNames, Color[] schemaColors, int selectedIndex)
            {
                _schemaNames = schemaNames;
                _schemaColors = schemaColors;
                _selectedIndex = selectedIndex;

                style.flexDirection = FlexDirection.Column;

                var labelElement = new Label(label);
                Add(labelElement);

                _dropdownButton = new VisualElement();
                _dropdownButton.style.flexDirection = FlexDirection.Row;
                _dropdownButton.style.alignItems = Align.Center;
                _dropdownButton.style.height = 18;
                _dropdownButton.style.backgroundColor = new StyleColor(BackgroundColor);
                SetBorder(_dropdownButton.style, 1, BorderColor);
                _dropdownButton.style.paddingLeft = 5;
                _dropdownButton.style.paddingRight = 5;

                UpdateSelectedDisplay();
                _dropdownButton.RegisterCallback<MouseDownEvent>(OnDropdownClicked);
                Add(_dropdownButton);
            }

            private void UpdateSelectedDisplay()
            {
                if (_dropdownButton == null || _schemaNames == null || _schemaColors == null) return;

                _dropdownButton.Clear();

                if (_selectedIndex >= 0 && _selectedIndex < _schemaNames.Length && _selectedIndex < _schemaColors.Length)
                {
                    var colorBox = new VisualElement();
                    colorBox.style.width = 16;
                    colorBox.style.height = 16;
                    colorBox.style.backgroundColor = new StyleColor(_schemaColors[_selectedIndex]);
                    SetBorder(colorBox.style, 1, GrayColor);
                    colorBox.style.marginRight = 5;

                    var label = new Label(_schemaNames[_selectedIndex]);
                    label.style.flexGrow = 1;

                    var arrow = new Label("▼");
                    arrow.style.fontSize = 10;
                    arrow.style.marginLeft = 5;

                    _dropdownButton.Add(colorBox);
                    _dropdownButton.Add(label);
                    _dropdownButton.Add(arrow);
                }
            }

            private void OnDropdownClicked(MouseDownEvent evt)
            {
                if (_schemaNames == null || _schemaColors == null) return;

                var popup = ScriptableObject.CreateInstance<ColorSchemaPopupWindow>();
                popup.Initialize(_schemaNames, _schemaColors, _selectedIndex);
                
                var buttonWorldBound = _dropdownButton.worldBound;
                var screenRect = UnityEditor.EditorGUIUtility.GUIToScreenRect(buttonWorldBound);
                var popupHeight = Mathf.Min(200, _schemaNames.Length * 22 + 10);
                popup.position = new Rect(screenRect.x, screenRect.yMax, screenRect.width, popupHeight);
                
                popup.OnItemSelected = (index) =>
                {
                    if (index != _selectedIndex)
                    {
                        _selectedIndex = index;
                        UpdateSelectedDisplay();
                        using (var changeEvent = ChangeEvent<string>.GetPooled(_schemaNames[_selectedIndex], _schemaNames[_selectedIndex]))
                        {
                            changeEvent.target = this;
                            SendEvent(changeEvent);
                        }
                    }
                    popup.Close();
                };
                
                popup.ShowPopup();
            }

            public void RegisterValueChangedCallback(EventCallback<ChangeEvent<string>> callback)
            {
                RegisterCallback(callback);
            }

            public string value => _selectedIndex >= 0 && _selectedIndex < _schemaNames.Length ? _schemaNames[_selectedIndex] : "";
        }

        private class ColorSchemaPopupWindow : EditorWindow
        {
            private string[] _schemaNames;
            private Color[] _schemaColors;
            private int _selectedIndex;
            public System.Action<int> OnItemSelected;

            public void Initialize(string[] schemaNames, Color[] schemaColors, int selectedIndex)
            {
                _schemaNames = schemaNames;
                _schemaColors = schemaColors;
                _selectedIndex = selectedIndex;
            }

            private void CreateGUI()
            {
                var root = rootVisualElement;
                root.style.backgroundColor = new StyleColor(BackgroundColor);
                SetBorder(root.style, 1, BorderColor);

                var scrollView = new ScrollView();
                scrollView.style.maxHeight = 200;

                for (var i = 0; i < _schemaNames.Length; i++)
                {
                    var index = i;
                    var item = new VisualElement();
                    item.style.flexDirection = FlexDirection.Row;
                    item.style.alignItems = Align.Center;
                    item.style.height = 20;
                    item.style.paddingLeft = 5;
                    item.style.paddingRight = 5;
                    item.style.paddingTop = 2;
                    item.style.paddingBottom = 2;

                    if (i == _selectedIndex)
                    {
                        item.style.backgroundColor = new StyleColor(SelectedBackgroundColor);
                    }

                    var colorBox = new VisualElement();
                    colorBox.style.width = 16;
                    colorBox.style.height = 16;
                    colorBox.style.backgroundColor = new StyleColor(i < _schemaColors.Length ? _schemaColors[i] : Color.white);
                    SetBorder(colorBox.style, 1, GrayColor);
                    colorBox.style.marginRight = 8;

                    var label = new Label(_schemaNames[i]);
                    label.style.flexGrow = 1;

                    item.Add(colorBox);
                    item.Add(label);

                    item.RegisterCallback<MouseDownEvent>(evt =>
                    {
                        evt.StopPropagation();
                        OnItemSelected?.Invoke(index);
                    });

                    item.RegisterCallback<MouseEnterEvent>(_ =>
                    {
                        item.style.backgroundColor = new StyleColor(HoverBackgroundColor);
                    });

                    item.RegisterCallback<MouseLeaveEvent>(_ =>
                    {
                        var selectedIndex = _selectedIndex;
                        item.style.backgroundColor = index == selectedIndex 
                            ? new StyleColor(SelectedBackgroundColor)
                            : StyleKeyword.Null;
                    });

                    scrollView.Add(item);
                }

                root.Add(scrollView);
                
                root.RegisterCallback<FocusOutEvent>(_ =>
                {
                    Close();
                });
            }
        }

        private VisualElement CreateTextField()
        {
            var textField = new TextField("Schema Name")
            {
                bindingPath = "schemaName"
            };
            textField.Bind(serializedObject);

            textField.RegisterValueChangedCallback(evt =>
            {
                serializedObject.ApplyModifiedProperties();
                if (_baseClass != null)
                {
                    ApplyColorToComponent(evt.newValue);
                }
            });

            return textField;
        }

        private void ApplyColorToComponent(string schemaName)
        {
            if (_baseClass == null || string.IsNullOrEmpty(schemaName)) return;

            var color = ColorSchemaUtils.GetColor(_baseClass, schemaName);
            var applier = target as IColorSchemaApplier;
            if (applier != null)
            {
                applier.ApplyColor(color);
            }
        }
    }
}
#endif

