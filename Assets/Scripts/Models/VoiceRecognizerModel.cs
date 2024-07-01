using LKZ.Commands.Voice;
using LKZ.DependencyInject;
using LKZ.TypeEventSystem;
using System;
using System.Collections;
using System.IO.Compression;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using UnityEngine;

namespace LKZ.Voice
{
    public sealed class VoiceRecognizerModel
    {
        const string url= "ws://1.94.131.28:19463/recognition";

        [Inject]
        private MonoBehaviour _mono { get; set; }

        [Inject]
        private ISendCommand SendCommand { get; set; }

        [Inject]
        private IRegisterCommand RegisterCommand { get; set; }


        private VoiceRecognitionResultCommand voiceRecognitionResult = new VoiceRecognitionResultCommand();

        VoiceRecognizerBase voiceRecognizer;
        public void Initialized()
        { 
            RegisterCommand.Register<SettingVoiceRecognitionCommand>(SettingVoiceRecognitionCommandCallback);

            voiceRecognizer = new VoiceRecognizerNoWebGL();
            voiceRecognizer.Initialized(this._mono, url, this.DisponseRecognition);
        }
         
        /// <summary>
        /// 配置语音识别命令回调
        /// </summary>
        /// <param name="obj"></param>
        private void SettingVoiceRecognitionCommandCallback(SettingVoiceRecognitionCommand obj)
        {
            voiceRecognizer.SetIsRecogition(obj.IsStartVoiceRecognition);
        }

        /// <summary>
        /// 处理识别结果
        /// </summary>
        /// <param name="count"></param>
        private void DisponseRecognition(string text1)
        {  
            if (text1 == " N" || text1 == "N" || text1 == "A" || text1 == " A")
                return;

            if (!string.IsNullOrEmpty(text1))
            {
                if (text1 == "\n")
                {
                    voiceRecognitionResult.IsComplete = true;
                    voiceRecognitionResult.text = string.Empty;
                }
                else
                {
                    voiceRecognitionResult.IsComplete = false;
                    voiceRecognitionResult.text = text1;
                }

                SendCommand.Send(voiceRecognitionResult);
            }
        }

        public void OnDestroy()
        { 
        }
    }


    public abstract class VoiceRecognizerBase
    {
        public abstract void Initialized(MonoBehaviour _mono,string websocketUrl, Action<string> _recognizerCallback);

        public abstract void SetIsRecogition(bool IsRecogition);
    } 

#if UNITY_EDITOR ||UNITY_STANDALONE || UNITY_ANDROID

    public sealed class VoiceRecognizerNoWebGL : VoiceRecognizerBase
    {
        ClientWebSocket webSocket;

        AudioClip microphoneClip;


        /// <summary>
        /// 采样麦克风间隔时间
        /// </summary>
        WaitForSeconds samplingInterval = new WaitForSeconds(1 / 5f);

        private MonoBehaviour _mono;

        private Action<string> recognizerCallback;
        private bool IsRecogition;

        public override async void Initialized(MonoBehaviour _mono, string websocketUrl, Action<string> _recognizerCallback)
        {
            this._mono = _mono;
            recognizerCallback = _recognizerCallback;

            webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri(websocketUrl), default);

            _mono.StartCoroutine(InitializedMicrophone());
            byte[] p = new byte[1024 * 1024];
            int count = 0;
            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(p, count, p.Length - count), default);
                count += result.Count;
                if (result.EndOfMessage)
                {
                    var str = Encoding.UTF8.GetString(p, 0, count);
                    recognizerCallback?.Invoke(str);
                    count = 0;
                }
            }
        }

        public override void SetIsRecogition(bool IsRecogition)
        {
            this.IsRecogition = IsRecogition;
            if (IsRecogition)
                this.lastSampling = Microphone.GetPosition(null);

        }

        IEnumerator InitializedMicrophone()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            {
                do
                {
                    microphoneClip = Microphone.Start(null, true, 1, 16000);
                    yield return null;
                } while (!Microphone.IsRecording(null));

                _mono.StartCoroutine(MicrophoneSamplingRecognition());
            }
            else
            {
                Debug.Log("请授权麦克风权限！");
            }
        }


        /// <summary>
        /// 上一次采样位置
        /// </summary>
        int lastSampling;

        float[] f = new float[16000];
        IEnumerator MicrophoneSamplingRecognition()
        {
            while (true)
            {
                yield return samplingInterval;
                if (!IsRecogition)
                    continue;

                int currentPos = Microphone.GetPosition(null);
                bool isSucceed = microphoneClip.GetData(f, 0);

                if (isSucceed)
                    if (lastSampling != currentPos)
                    {
                        int count = 0;
                        float[] p = default;
                        if (currentPos > lastSampling)
                        {
                            count = currentPos - lastSampling;
                            p = new float[count]; 
                            Array.Copy(f, lastSampling, p, 0, count);
                        }
                        else
                        {
                            count = 16000 - lastSampling;
                            p = new float[count + currentPos]; 
                            Array.Copy(f, lastSampling, p, 0, count);
                             
                            Array.Copy(f, 0, p, count, currentPos);

                            count += currentPos;
                        }

                        lastSampling = currentPos;
                        DisponseRecognition(p);
                    }

            }
        }

        private void DisponseRecognition(float[] p)
        {
            var buffer = FloatArrayToByteArray(p);

            
            this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, default);
        }


        byte[] FloatArrayToByteArray(in float[] floatArray)
        {
            int byteCount = floatArray.Length * sizeof(float);
            byte[] byteArray = new byte[byteCount];

            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteCount);

            return byteArray;
        }

        static byte[] Compress(in byte[] data)
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return compressedStream.ToArray();
            }
        }
    }
#endif
}