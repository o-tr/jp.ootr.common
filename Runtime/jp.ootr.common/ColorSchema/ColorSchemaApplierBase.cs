#if UNITY_EDITOR
using jp.ootr.common.Editor;
using UnityEngine;

namespace jp.ootr.common.ColorSchema
{
    public class ColorSchemaApplierBase : MonoBehaviour, IColorSchemaApplier
    {
        [SerializeField] protected string schemaName;

        public void OnDrawGizmos()
        {
            GizmoIcons.DisableGizmos();
        }

        public string SchemaName => schemaName;

        public virtual void ApplyColor(Color color)
        {
        }
    }
}
#endif
