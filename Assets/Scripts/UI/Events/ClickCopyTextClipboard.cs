using LKZ.DependencyInject;
using UnityEngine;
using UnityEngine.UI;

namespace LKZ.UI.Events
{
    [RequireComponent(typeof(Button))]
    internal sealed class ClickCopyTextClipboard : MonoBehaviour
    {
        public Text copyText;
         
        public void Start()
        {
            SceneDependencyInjectContextManager.Instance.InjectProperty(this);
            GetComponent<Button>().onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            Utilitys.Clipboard.CopyClipboard(copyText.text); 
        }
    }
}
