using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 创建依赖注入的Unity配置文件
    /// </summary>
    public class CreateScriptableObject
    {
        [MenuItem("Assets/Create/DI/ScriptableObject", priority = 5000, validate = false)]
        public static void jj()
        {
            string path = EditorUtility.SaveFilePanel("创建Unity序列化文件", AssetDatabase.GetAssetPath(Selection.activeObject),
                  "GameSetting", "cs");

            if (!string.IsNullOrEmpty(path))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder
                    .Append("using LKZ.DependencyInject;\r\n")
                    .Append("using UnityEngine; \r\n")
                    .Append("\r\n")
                    .Append(@"/// <summary>
/// Unity 配置文件
/// 这个需要拖到SceneDependencyInjectContextManager脚本中的GameSetting下，才能启动被绑定
/// </summary>")
    .Append("\r\n[CreateAssetMenu(menuName =\"GameSetting/\"+nameof(FileName))]\r\n")
    .Append("public class FileName : DIScriptableObject\r\n")
    .Append("{\r\n")
    .Append("  /// <summary>\r\n")
    .Append("   /// 启动的时候，会自动调用这个来注入绑定依赖\r\n")
            .Append("   /// </summary>\r\n")
        .Append("   /// <param name=\"registerBinding\">绑定接口</param>\r\n")
            .Append("   public override void InjectBinding(IRegisterBinding registerBinding)\r\n")
            .Append("   {\r\n")
            .Append("        registerBinding.BindingToSelf(this);//绑定自身\r\n")
            .Append("   }\r\n")
            .Append("}\r\n");
                stringBuilder.Replace("FileName", Path.GetFileNameWithoutExtension(path));
                File.WriteAllText(path, stringBuilder.ToString());
                AssetDatabase.Refresh();
            }
        }
    }
}
