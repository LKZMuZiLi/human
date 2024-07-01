using LKZ.TypeEventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 层级面板可以右键创建类型事件系统管理
/// </summary>
public class CreateTypeEventSystemManager
{
    [UnityEditor.MenuItem("GameObject/LKZ/创建事件系统管理者", priority = 11)]
    private static void CreateEventSystemManager()
    {
        if (Object.FindObjectOfType<TypeEventSystemManager>() != null)
        {
            Debug.LogError("场景中已经有一个类型事件系统管理，不需要创建");
        }
        else
        {
            GameObject eventSystemGo = new GameObject("TypeEventSystemManager");
            eventSystemGo.AddComponent<TypeEventSystemManager>();
            Selection.activeObject = eventSystemGo;
        }
    }
}
