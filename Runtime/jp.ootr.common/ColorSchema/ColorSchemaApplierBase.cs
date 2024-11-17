using UnityEngine;

namespace jp.ootr.common.ColorSchema
{
    public class ColorSchemaApplierBase : MonoBehaviour, IColorSchemaApplier
    {
        [SerializeField] protected string schemaName;

        public void OnDrawGizmos()
        {
        }

        public string SchemaName => schemaName;

        public virtual void ApplyColor(Color color)
        {
        }
    }
}
