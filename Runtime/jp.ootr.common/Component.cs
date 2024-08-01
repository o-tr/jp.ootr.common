using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace jp.ootr.common
{
    public class ComponentUtils
    {
        public static List<T> GetAllComponents<T>()
        {
            var components = new List<T>();
            var scene = SceneManager.GetActiveScene();
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                components.AddRange(rootObject.GetComponentsInChildren<T>());
            }
            return components;
        }
    }
}