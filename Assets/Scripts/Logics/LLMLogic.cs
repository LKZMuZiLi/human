using LKZ.Chat.Commands;
using LKZ.Commands.Chat;
using LKZ.Commands.Voice;
using LKZ.DependencyInject;
using LKZ.Models;
using LKZ.TypeEventSystem;
using LKZ.VoiceSynthesis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LKZ.Logics
{

    public sealed class LLMLogic
    {

        private sealed class ResultData : IEnumerator
        {
            public string result;
            public IEnumerator clip;

            public object Current => current;// clip.Current;

            object current = null;

            public bool MoveNext()
            {
                if (!(clip.Current is AudioClip))
                {
                    current = clip.Current;
                    return true;
                }
                else if (clip.Current is string str)
                    return str != VoiceTTS.ErrorMess;
                else
                    return false;
            }

            public void Reset()
            {

            }
        }

        [Inject]
        private AudioModel audioModel { get; set; }

        [Inject]
        private MonoBehaviour _mono { get; set; }

        [Inject]
        private ISendCommand SendCommand { get; set; }

        [Inject]
        private IRegisterCommand RegisterCommand { get; set; }

        /// <summary>
        /// GPT播放语音片段
        /// </summary>
        Queue<ResultData> gptVoice = new Queue<ResultData>();

        Action<string> _showUITextAction;


        private string onceResult;

        /// <summary>
        /// 是否接收完成GPT的内容
        /// </summary>
        private bool isRequestChatGPTContent, isStopCreate;

        /// <summary>
        /// 字幕同步携程
        /// </summary>
        private Coroutine _titleSynchronization_Cor;

        /// <summary>
        /// 字幕同步携程
        /// </summary>
        private Coroutine _requestGPTSegmentationCor;


        ClientWebSocket webSocket;

        LLMConfig config;
        public void Initialized()
        {
            RegisterCommand.Register<StopGenerateCommand>(StopGenerateCommandCallback);
            config = Resources.Load<LLMConfig>("LLMConfig");
            ConnectServer();

        }

        async void ConnectServer()
        {
            while (true)
            {
                try
                {
                    webSocket = new ClientWebSocket();

                    await webSocket.ConnectAsync(new Uri( config.uri), default);

                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(config.userName), WebSocketMessageType.Binary, true, default);
                    break;
                }
                catch
                { 
                    await Task.Delay(1000);
                }
            }

            ReceiveData();


        }

        async void ReceiveData()
        {
            byte[] p = new byte[1024 * 1024];
            int count = 0;

            try
            {
                while (true)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(p, count, p.Length - count), default);
                    count += result.Count;
                    if (result.EndOfMessage)
                    {
                        var str = Encoding.UTF8.GetString(p, 0, count);
                        try
                        {
                            Debug.Log(str);
                            var t = Newtonsoft.Json.Linq.JToken.Parse(str);
                            var data = t["Data"];
                            var key = data["Key"].ToString();
                            if (key == "question")
                            {
                                SendCommand.Send(new AddChatContentCommand { infoType = Enum.InfoType.My, _addTextAction = value => _showUITextAction = value });
                                _showUITextAction?.Invoke(data["Value"].ToString());
                                isStopCreate = false;
                            }
                            else if (key == "audio")
                            {
                                SendCommand.Send(new AddChatContentCommand { infoType = Enum.InfoType.ChatGPT, _addTextAction = value => _showUITextAction = value });

                                ClearGPTVoice();
                                ChatGPTRequestCallback(data["HttpValue"].ToString(), data["Text"].ToString(), true);
                                isStopCreate = false;

                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }



                        count = 0;
                    }
                }
            }
            catch
            {
                ConnectServer();
            }

        }

        private void StopGenerateCommandCallback(StopGenerateCommand obj)
        {
            if (!object.ReferenceEquals(null, _titleSynchronization_Cor))
                _mono.StopCoroutine(_titleSynchronization_Cor);
            if (!object.ReferenceEquals(null, _requestGPTSegmentationCor))
                _mono.StopCoroutine(_requestGPTSegmentationCor);

            _titleSynchronization_Cor = null;
            _requestGPTSegmentationCor = null;

            PlayFinish();
        }
         
        private void ChatGPTRequestCallback(string url, string arg1, bool arg2)
        {
            if (!string.IsNullOrEmpty(arg1))
                _mono.StartCoroutine(SynthesisCoroutine(url, arg1));

            isRequestChatGPTContent = arg2;

        }
         
        private IEnumerator SynthesisCoroutine(string url, string text)
        {
            IEnumerator youdaoIE = VoiceTTS.Synthesis(url);

            gptVoice.Enqueue(new ResultData { clip = youdaoIE, result = text });

            yield return youdaoIE;
            if (_titleSynchronization_Cor == null)
                _titleSynchronization_Cor = _mono.StartCoroutine(TitleSynchronizationCoroutine());

        }

        /// <summary>
        /// 字幕同步协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator TitleSynchronizationCoroutine()
        {
            SendCommand.Send(new ChatGPTStartTalkCommand());//开始播放

            int lastIndex = -1;
            while (!isStopCreate && (gptVoice.Count > 0 || !isRequestChatGPTContent))
            {
                if (gptVoice.Count == 0)
                {
                    yield return null;
                    continue;
                }
                ResultData result = gptVoice.Dequeue(); 
                yield return result;
                if (result.clip.Current is AudioClip clip)
                {
                    audioModel.Play(clip);

                    int temp_Index = 0;
                    yield return null;
                    yield return null;


                    while (true)
                    {
                        if (audioModel.Time == clip.length || !audioModel.IsPlaying || _showUITextAction == null)
                            break;

                        temp_Index = (int)((audioModel.Time / clip.length) * result.result.Length) - 1;
                        temp_Index = Mathf.Clamp(temp_Index, 0, result.result.Length);

                        if (lastIndex != temp_Index)
                        {
                            string str = result.result[temp_Index].ToString();
                            _showUITextAction(str);
                            lastIndex = temp_Index;
                        }

                        yield return null;
                    }


                    GameObject.Destroy(clip);

                    if (result.result.Length > ++temp_Index)
                    {
                        string str = result.result[temp_Index].ToString();
                        _showUITextAction(str);
                    }
                }
            }


            PlayFinish();
        }

        void ClearGPTVoice()
        {
            while (gptVoice.Count > 0)
            {
                var item = gptVoice.Dequeue();
                if (item.clip.Current is AudioClip clip)
                    GameObject.Destroy(clip);

            }
        }

        /// <summary>
        /// 播放完成
        /// </summary>
        private void PlayFinish()
        {
            isStopCreate = true;

            _titleSynchronization_Cor = null;
            ClearGPTVoice();


            SendCommand.Send(new GenerateFinishCommand { });//生成完成命令

            _showUITextAction = null;

            audioModel.Stop();
        }
    }
}
