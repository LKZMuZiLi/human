using LKZ.Chat.Commands;
using LKZ.Commands.Chat;
using LKZ.Commands.Voice;
using LKZ.DependencyInject;
using LKZ.Logics;
using LKZ.TypeEventSystem;
using LKZ.Voice;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LKZ.Manager
{

    public sealed class GameApp : MonoBehaviour, DIAwakeInterface, IDRegisterBindingInterface
    {
        [SerializeField, TextArea]
        private string StartContent =
@"我是一个由木子李开发的GPT聊天机器人
你可以语音和我聊天
如果你有其他需求，请联系wx:LKZ4251";


        private VoiceRecognizerModel voiceRecognizer;
        private LLMLogic  llmLogic;

        [Inject]
        private ISendCommand SendCommand { get; set; }

        public void DIRegisterBinding(IRegisterBinding registerBinding)
        {
            registerBinding.Binding<MonoBehaviour>().To(this);


            voiceRecognizer = new VoiceRecognizerModel();
            registerBinding.Binding<VoiceRecognizerModel>().To(voiceRecognizer);

            llmLogic = new LLMLogic();
            registerBinding.Binding<LLMLogic>().To(llmLogic);

        }

        void DIAwakeInterface.OnAwake()
        { 
            Application.runInBackground = true;

            Input.backButtonLeavesApp = true;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;


            Screen.fullScreen = false; 
        }
         

        private void Start()
        {  
            SceneDependencyInjectContextManager.Instance.InjectProperty(voiceRecognizer);
            SceneDependencyInjectContextManager.Instance.InjectProperty(llmLogic);
            voiceRecognizer.Initialized();
            llmLogic.Initialized();

            SendCommand.Send(new AddChatContentCommand { infoType = Enum.InfoType.ChatGPT, _addTextAction = value => value.Invoke(StartContent) });
            SendCommand.Send(new GenerateFinishCommand { });//生成完成命令
             
            SendCommand.Send(new SettingVoiceRecognitionCommand { IsStartVoiceRecognition = true });//开始语音识别

        }


        private void OnDestroy()
        {
            voiceRecognizer.OnDestroy();
        }
    }
}
