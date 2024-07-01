using UnityEngine;

namespace LKZ.Logics
{
    [CreateAssetMenu(fileName = nameof(LLMConfig), menuName = "大模型配置表")]
    internal sealed class LLMConfig : ScriptableObject
    {
        [Header("大模型平台关键配置")]
        [Tooltip("大模型地址")]
        public string url;

        [Tooltip("调用哪个模型")]
        public string model;

        [Tooltip("大模型平台的key")]
        public string key;

        [Header("角色预设")]
        [TextArea]
        public string roleSetting;

    }
}
