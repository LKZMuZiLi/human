#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices; 
#endif

using UnityEngine;

namespace LKZ.Utilitys
{
    internal static class Clipboard
    {

#if !UNITY_EDITOR && UNITY_WEBGL
        /// <summary>
        /// 复制文本到剪切板
        /// </summary>
        /// <param name="text"></param>
        [DllImport("__Internal" )]
        private static extern void CopyToClipboard(string text);
#endif

        public static void CopyClipboard(string text)
        { 
#if !UNITY_EDITOR && UNITY_WEBGL
           CopyToClipboard(text);  
#else
            UnityEngine.GUIUtility.systemCopyBuffer = text; 
#endif

        }
    }
}
