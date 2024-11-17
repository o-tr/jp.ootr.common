using UnityEngine;

namespace jp.ootr.common.ColorSchema
{
    internal interface IColorSchemaApplier
    {
        string SchemaName { get; }
        void ApplyColor(Color color);
    }
}
