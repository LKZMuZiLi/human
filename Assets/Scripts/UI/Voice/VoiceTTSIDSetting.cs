using LKZ.DependencyInject;
using LKZ.VoiceSynthesis;
using UnityEngine;
using UnityEngine.UI;

namespace LKZ.UI.Voice
{
    internal sealed class VoiceTTSIDSetting : MonoBehaviour, DIAwakeInterface
    {
        public int voiceID;

        public string voiceShowName; 

        public void OnAwake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(ToggleClick);

        }

        private void ToggleClick(bool arg0)
        {
            if(arg0  )
            {
                VoiceTTS.VoiceID = voiceID; 
            }
        }
    }
}
