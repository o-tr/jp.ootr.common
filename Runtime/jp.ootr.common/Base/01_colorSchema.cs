using UnityEngine;
using UnityEngine.Serialization;

namespace jp.ootr.common.Base
{
    public class BaseClass__ColorSchema : BaseClass__Base
    {
        [FormerlySerializedAs("colorSchemeNames")] [SerializeField]
        internal string[] colorSchemaNames;

        [FormerlySerializedAs("colorSchemes")] [SerializeField]
        internal Color[] colorSchemas;
    }
}
