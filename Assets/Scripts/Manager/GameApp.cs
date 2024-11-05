using LKZ.Chat.Commands;
using LKZ.Commands.Chat;
using LKZ.Commands.Voice;
using LKZ.DependencyInject;
using LKZ.Logics;
using LKZ.TypeEventSystem;
using UnityEngine;

namespace LKZ.Manager
{

    public sealed class GameApp : MonoBehaviour, DIAwakeInterface, IDRegisterBindingInterface
    {
        [SerializeField, TextArea]
        private string StartContent =
@"����һ����ľ�������GPT���������
�����������������
�������������������ϵwx:LKZ4251";

         
        private LLMLogic  llmLogic;

        [Inject]
        private ISendCommand SendCommand { get; set; }

        public void DIRegisterBinding(IRegisterBinding registerBinding)
        {
            registerBinding.Binding<MonoBehaviour>().To(this);

             

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
            SceneDependencyInjectContextManager.Instance.InjectProperty(llmLogic); 
            llmLogic.Initialized();

            SendCommand.Send(new AddChatContentCommand { infoType = Enum.InfoType.ChatGPT, _addTextAction = value => value.Invoke(StartContent) });
            SendCommand.Send(new GenerateFinishCommand { });//�����������
             
            SendCommand.Send(new SettingVoiceRecognitionCommand { IsStartVoiceRecognition = true });//��ʼ����ʶ��

        }


        private void OnDestroy()
        { 
        }
    }
}
