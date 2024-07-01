using LKZ.Chat.Commands;
using LKZ.Commands.Chat;
using LKZ.DependencyInject;
using LKZ.Enum;
using LKZ.TypeEventSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LKZ.UI
{
    public sealed class ScrollViewGPTContent : MonoBehaviour, DIStartInterface, DIAwakeInterface, IBeginDragHandler, IEndDragHandler
    {
        [Inject]
        private IRegisterCommand RegisterCommand { get; set; }

        ScrollRect _scrollRect;

        private RectTransform _scrollRect_Content;

        [SerializeField]
        private GameObject _my_Go, _gpt_go;

        [SerializeField, Tooltip("间隔")]
        private float interval = 15f;

        [SerializeField, Tooltip("生成数据是否往上移动多少")]
        private float GPTGenerateContentUpMove = 30f;

        private ShowContent currentShowContent;

        /// <summary>
        /// 生成内容和不生成内容滚动视图的位置
        /// </summary>
        private Vector3 defaultPos, GPTGenerateContentPos;
         

        private bool isSetScrollRectNormalizedPosition;

        /// <summary>
        /// 是否再生成GPT内容
        /// </summary>
        private bool isGenerateGPTContent;

        private RectTransform thisRect;

        void DIAwakeInterface.OnAwake()
        {
            thisRect = base.transform as RectTransform;

            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect_Content = _scrollRect.content;

            defaultPos = thisRect.anchoredPosition;
            GPTGenerateContentPos = defaultPos;
            GPTGenerateContentPos.y += GPTGenerateContentUpMove;
             
            isSetScrollRectNormalizedPosition = true;
        }

        void DIStartInterface.OnStart()
        {
            RegisterCommand.Register<AddChatContentCommand>(AddChatContentCommandCallback);

            RegisterCommand.Register<GenerateFinishCommand>(GenerateFinishCommandCallback); 

        }
         

        private void GenerateFinishCommandCallback(GenerateFinishCommand obj)
        {
            isGenerateGPTContent = false;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            isSetScrollRectNormalizedPosition = true;

        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            isSetScrollRectNormalizedPosition = false;

        }

        private void OnDestroy()
        {
            RegisterCommand.UnRegister<AddChatContentCommand>(AddChatContentCommandCallback);
        }

        private void AddChatContentCommandCallback(AddChatContentCommand obj)
        {
            Vector2 pos = new Vector2(0, -_scrollRect_Content.sizeDelta.y);

            if (!object.ReferenceEquals(null, currentShowContent))
                currentShowContent.Enabled = false;

            switch (obj.infoType)
            {
                case InfoType.My:
                    currentShowContent = Instantiate(_my_Go, _scrollRect_Content).GetComponent<ShowContent>();
                  //  pos.x = ScreenWidth;
                    break;
                case InfoType.ChatGPT:
                    currentShowContent = Instantiate(_gpt_go, _scrollRect_Content).GetComponent<ShowContent>();
                    isGenerateGPTContent = true;
                    break;
            }

            currentShowContent.Initialized(pos);

            lastHeight = 0;

            _scrollRect_Content.sizeDelta += new Vector2(0, interval);

            obj._addTextAction(AddShowText);
        }

        float lastHeight = default;

        private void AddShowText(string c)
        {
            currentShowContent.AddText(c);
        }

        private void LateUpdate()
        {
            if (object.ReferenceEquals(null, currentShowContent))
                return;

            if (currentShowContent.Height != lastHeight)
            {
                _scrollRect_Content.sizeDelta += new Vector2(0, currentShowContent.Height - lastHeight);

                lastHeight = currentShowContent.Height;
            }

            if (isSetScrollRectNormalizedPosition)
                _scrollRect.verticalNormalizedPosition = Mathf.Lerp(_scrollRect.verticalNormalizedPosition, 0, 0.05f);

#if !UNITY_STANDALONE_WIN
            //处理往上移动
            thisRect.anchoredPosition = Vector3.Lerp(this.thisRect.anchoredPosition, isGenerateGPTContent ? this.GPTGenerateContentPos : this.defaultPos, 0.05f);
#endif
        }


    }
}