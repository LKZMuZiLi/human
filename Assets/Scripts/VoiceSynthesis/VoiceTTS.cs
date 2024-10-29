using LKZ.Utilitys;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace LKZ.VoiceSynthesis
{ 

    /// <summary>
    ///  语音合成
    /// </summary>
    public static class VoiceTTS
    {
        public const string ErrorMess = "语音识别出错了";

        static int voiceID;
        public static int VoiceID
        {
            get => voiceID; set
            {
                voiceID = value;
                DataSave.SetVoiceID(voiceID);
            }
        }

        static VoiceTTS()
        {
            voiceID = DataSave.GetVoiceID();
        }

        public static IEnumerator Synthesis(string url)
        { 
            using (var request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            { 
                var result = request.SendWebRequest();
                while (!result.isDone)
                {
                    yield return null;
                }
                if (string.IsNullOrEmpty(request.error))
                {
                    AudioClip _audioClip = DownloadHandlerAudioClip.GetContent(request);
                    yield return _audioClip;
                }
                else
                {
                    yield return ErrorMess;
                }
            }
        }
    }
}
