using LKZ.Commands.Chat;
using LKZ.Commands.Voice;
using LKZ.DependencyInject;
using LKZ.TypeEventSystem;
using System;
using System.Collections;
using UnityEngine;

namespace LKZ.Rolle
{
    public interface IRolleManager
    {
        public int RolleCount { get; }
        public int CurrentIndex { get; }

    }

    public interface IRollePosition
    {
        public Vector3 Position { get; }

        /// <summary>
        /// 角色显示的位置
        /// </summary>
        public Vector3 RolleShowPosition { get; }
    }

    public sealed class RolleManager : MonoBehaviour, IRolleManager, IRollePosition, IDRegisterBindingInterface, DIAwakeInterface
    { 
        [SerializeField]
        private Vector3 rolleMiddlePos; 

        [SerializeField]
        private GameObject[] RolleGames;

        public float switchRolleSpeed = 1f;

        private RolleControl[] RolleControls;

        private int rolleIndex = 0;

        [Inject]
        private IRegisterCommand RegisterCommand { get; set; }


        public AudioSource AudioSource => RolleControls[rolleIndex].AudioSource;

        public int RolleCount => RolleControls.Length;

        public int CurrentIndex => rolleIndex;

        public Vector3 Position => RolleControls[rolleIndex].transform.position;

        public Vector3 RolleShowPosition => rolleMiddlePos;
          

        public float switchDancePosResetSpeed = 2f;


        static readonly Quaternion defaultRotation = Quaternion.Euler(0, 180, 0);
        public void OnAwake()
        {
            RolleControls = new RolleControl[RolleGames.Length];

            for (int i = 0; i < RolleControls.Length; i++)
            {
                RolleControls[i] = Instantiate(this.RolleGames[i], this.rolleMiddlePos, Quaternion.Euler(0, 180, 0), base.transform).GetComponent<RolleControl>();
                RolleControls[i].RolleDisable();
            }

            RolleControls[0].RolleEnable();
             

            RegisterCommand.Register<ChatGPTStartTalkCommand>(ChatGPTStartTalkCommandCallback);
            RegisterCommand.Register<SettingVoiceRecognitionCommand>(SettingVoiceRecognitionCommandCallback); 
        }

        public float PlaySaluteAnimationDelay = 1f;
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(PlaySaluteAnimationDelay);
            RolleControls[0].TriggerSaluteAnimation();
        }

         
        void IDRegisterBindingInterface.DIRegisterBinding(IRegisterBinding registerBinding)
        {
            registerBinding.Binding<IRolleManager>().To(this);
            registerBinding.Binding<IRollePosition>().To(this);
        }

        private bool isSwitch;
          
        private void SettingVoiceRecognitionCommandCallback(SettingVoiceRecognitionCommand obj)
        {
            if (obj.IsStartVoiceRecognition)
                RolleControls[this.rolleIndex].SetAnimationTalk(false);
        }

        private void ChatGPTStartTalkCommandCallback(ChatGPTStartTalkCommand obj)
        {
            RolleControls[this.rolleIndex].SetAnimationTalk(true);
        }
         
           
    }
}
