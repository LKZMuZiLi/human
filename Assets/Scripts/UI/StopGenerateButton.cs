using LKZ.Chat.Commands;
using LKZ.Commands.Chat;
using LKZ.DependencyInject;
using LKZ.TypeEventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace LKZ.UI
{
    /// <summary>
    /// ui停止生成按钮
    /// </summary>
    public sealed class StopGenerateButton : MonoBehaviour, DIStartInterface
    {

        [Inject]
        private IRegisterCommand RegisterCommand { get; set; }

        [Inject]
        private ISendCommand SendCommand { get; set; }

        void DIStartInterface.OnStart()
        {
            RegisterCommand.Register<AddChatContentCommand>(AddChatContentCommandCallback);
            RegisterCommand.Register<GenerateFinishCommand>(GenerateFinishCommandCallback);

            GetComponent<Button>().onClick.AddListener(() => SendCommand.Send(new StopGenerateCommand()));
        }

        private void GenerateFinishCommandCallback(GenerateFinishCommand obj)
        {
            gameObject.SetActive(false);
        }

        private void AddChatContentCommandCallback(AddChatContentCommand obj)
        {
            if (obj.infoType == Enum.InfoType.ChatGPT)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
