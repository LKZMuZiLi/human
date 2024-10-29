using UnityEngine;

namespace LKZ.Logics
{
    [CreateAssetMenu(fileName = nameof(LLMConfig), menuName = "大模型配置表")]
    internal sealed class LLMConfig : ScriptableObject
    {
        [Header("大模型平台关键配置")]
        [Tooltip("fay地址")]
        public string uri;

        [Tooltip("fay用户名")]
        public string userName;
         

    }
}
