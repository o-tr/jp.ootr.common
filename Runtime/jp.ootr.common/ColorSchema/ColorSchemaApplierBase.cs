using UnityEngine;

namespace jp.ootr.common.ColorSchema
{
    public class ColorSchemaApplierBase : MonoBehaviour, IColorSchemaApplier
    {
        [SerializeField] protected string schemaName;
        
        public string SchemaName => schemaName;

        public void OnDrawGizmos()
        {
            return;
        }

        public virtual void ApplyColor(Color color)
        {
        }
    }
}
