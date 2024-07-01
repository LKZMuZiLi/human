using UnityEditor;
using UnityEngine;

namespace LKZ.DependencyInject
{
    public class CreateSceneDependencyInjectManager
    {
        [UnityEditor.MenuItem("GameObject/LKZ/创建依赖注入场景管理", priority = 11)]
        private static void CreateEventSystemManager()
        {
            if (Object.FindObjectOfType<SceneDependencyInjectContextManager>() != null)
            {
                Debug.LogError("场景中已经有一个依赖注入管理上下文，不需要创建");
            }
            else
            {
                GameObject DIContext = new GameObject(nameof(SceneDependencyInjectContextManager));
                DIContext.AddComponent<SceneDependencyInjectContextManager>();
                Selection.activeObject = DIContext;
            }
        }
    }
}
