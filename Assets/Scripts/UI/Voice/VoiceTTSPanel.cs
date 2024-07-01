using LKZ.Utilitys;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LKZ.UI.Voice
{
    internal sealed  class VoiceTTSPanel : MonoBehaviour 
    { 

        private IEnumerator Start()
        {
            yield return null;
            var id = DataSave.GetVoiceID();
            foreach (Transform item in base.transform.GetChild(0))
            {
                var temp = item.GetComponent<VoiceTTSIDSetting>();
                if (id == temp.voiceID)
                {
                    item.GetComponent<Toggle>().isOn = true;
                }
            }
        }
    }
}
