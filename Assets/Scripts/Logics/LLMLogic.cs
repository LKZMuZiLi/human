using LKZ.Chat.Commands;
using LKZ.Commands.Chat;
using LKZ.Commands.Voice;
using LKZ.DependencyInject;
using LKZ.GPT;
using LKZ.Models;
using LKZ.TypeEventSystem;
using LKZ.VoiceSynthesis;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Initialized()
        {
            RegisterCommand.Register<VoiceRecognitionResultCommand>(VoiceRecognitionResultCommandCallback);
            
            RegisterCommand.Register<StopGenerateCommand>(StopGenerateCommandCallback);
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

        /// <summary>
        /// 语音识别到内容回调
        /// </summary>
        /// <param name="obj"></param>
        private void VoiceRecognitionResultCommandCallback(VoiceRecognitionResultCommand obj)
        {
            if (!obj.IsComplete)
            {
                if (_showUITextAction == null)
                    SendCommand.Send(new AddChatContentCommand { infoType = Enum.InfoType.My, _addTextAction = value => _showUITextAction = value });

                _showUITextAction.Invoke(obj.text);

                onceResult += obj.text;
            }
            else
            {
                if (string.IsNullOrEmpty(onceResult))
                    return;

                SendCommand.Send(new SettingVoiceRecognitionCommand { IsStartVoiceRecognition = false });//停止语音识别

                SendCommand.Send(new AddChatContentCommand { infoType = Enum.InfoType.ChatGPT, _addTextAction = value => _showUITextAction = value });

                ClearGPTVoice();

                 _requestGPTSegmentationCor = _mono.StartCoroutine(LLM.Request(onceResult, ChatGPTRequestCallback));
                onceResult = string.Empty;
                isStopCreate = false;
            }
        }

        private void ChatGPTRequestCallback(string arg1, bool arg2)
        {
            if (!string.IsNullOrEmpty(arg1))
                _mono.StartCoroutine(SynthesisCoroutine(arg1));
             
            isRequestChatGPTContent = arg2;

        }


        private IEnumerator SynthesisCoroutine(string text)
        {
            IEnumerator youdaoIE = VoiceTTS.Synthesis(text);

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

            SendCommand.Send(new SettingVoiceRecognitionCommand { IsStartVoiceRecognition = true });//开始语音识别
            SendCommand.Send(new GenerateFinishCommand { });//生成完成命令

            _showUITextAction = null;

            audioModel.Stop();
        }
    }
}
