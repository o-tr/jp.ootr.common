using UnityEngine;

namespace jp.ootr.common.ColorSchema
{
    interface IColorSchemaApplier
    {
        string SchemaName { get; }
        void ApplyColor(Color color);
    }
}
