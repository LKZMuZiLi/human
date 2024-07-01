using UnityEngine;
using UnityEngine.UI;

namespace LKZ.UI 
{
    public sealed class ShowContent : MonoBehaviour
    {
        [SerializeField, Tooltip("文本最大宽度")]
        private float textMaxWidth = 755f;

        [SerializeField, Tooltip("父物体的格外大小")]
        private Vector2 _parentExtraSize = new Vector2(5f, 5f);

        [SerializeField, Tooltip("复制按钮高度")]
        private float coptButtonHeight;

        public Text _text;

        private RectTransform _textParent;

        private LayoutElement _textLayout;

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
                GetComponentInChildren<ContentSizeFitter>().enabled = value;
                GetComponentInChildren<LayoutElement>().enabled = value;
            }
        }


        public float Height
        {
            get
            {
                return _textParent.rect.height + coptButtonHeight;
            }
        }



        public void Initialized(Vector2 position)
        {
            var _rect = GetComponent<RectTransform>();

            _textLayout = _text.GetComponent<LayoutElement>();
            _textParent = _text.rectTransform.parent.GetComponent<RectTransform>();

            _rect.localScale = Vector3.one;
            _rect.anchoredPosition = position;
        }


        public void AddText(in string str)
        {
            _text.text += str;
        }


        private void LateUpdate()
        {
            var rect = _text.rectTransform.rect;

            if (rect.width >= textMaxWidth && _textLayout.preferredWidth != textMaxWidth)
            {
                _textLayout.preferredWidth = textMaxWidth;
                return;
            }
            _textParent.sizeDelta = new Vector2(rect.width, rect.height) + _parentExtraSize;
        }
    }
}
