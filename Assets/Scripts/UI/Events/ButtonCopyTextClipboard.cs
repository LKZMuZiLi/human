using LKZ.DependencyInject;
using LKZ.Utilitys;
using UnityEngine;
using UnityEngine.UI;

namespace LKZ.UI.Events
{
    [RequireComponent(typeof(Button))]
    internal sealed class ButtonCopyTextClipboard : MonoBehaviour, DIStartInterface
    {
        public string copyText;
         
        public void OnStart()
        {
            GetComponent<Button>().onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            Clipboard.CopyClipboard(copyText); 
        }
    }
}
