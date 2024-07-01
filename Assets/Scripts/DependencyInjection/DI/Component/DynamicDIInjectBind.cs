using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 动态依赖注入绑定
    /// 这个要跟着场景一起启动的
    /// 如果想场景启动了，动态给场景添加脚本自动注入
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("LKZ/依赖注入/动态绑定依赖注入", order: 11)]
    public class DynamicDIInjectBind : MonoBehaviour
    {
        /// <summary>
        /// 是否销毁了取消注册
        /// </summary>
        public bool isDestroyUnRegister = true;

        /// <summary>
        /// 本物体使用了DynamicInjectBindAttribute特性的类
        /// </summary>
        private object[] useDynamicInjectBindComponent;

        /// <summary>
        /// 依赖注入绑定的接口
        /// </summary>
        private IRegisterBinding registerBinding;

        /// <summary>
        /// 本物体上的自定义脚本
        /// </summary>
        private  object[] customComponent;

        /// <summary>
        /// 动态的绑定特性标识类型
        /// </summary>
        private readonly static Type DynamicInjectBindAttributeType = typeof(DynamicInjectBindAttribute);

        private void Start()
        {
            Inject();
        }

        /// <summary>
        /// 注入
        /// </summary>
        public void Inject()
        {
            MonoBehaviour[] monos = GetComponents<MonoBehaviour>();

            customComponent = FilteationUtility.FiltrationComponent(monos);

            registerBinding = SceneDependencyInjectContextManager.Instance.GetRegisterBindingInterface();
            useDynamicInjectBindComponent = customComponent.GetUseDynamicInjectBindAttribute();

            //给本物体上的所有了要绑定的脚本绑定注入到DI
            foreach (object item in useDynamicInjectBindComponent)
            {
                var att = item.GetType().GetCustomAttributes(DynamicInjectBindAttributeType, false)[0] as DynamicInjectBindAttribute;
                switch (att.BindingType)
                {
                    case BindingType.BindingToSelf:
                        registerBinding.BindingToSelf(item);
                        break;
                    case BindingType.BindingToSelfAndAllInterface:
                        registerBinding.BindingToSelfAndAllInterface(item);
                        break;
                    case BindingType.BindingToAllInterface:
                        registerBinding.UnBindingToAllInterface(item.GetType());
                        break;
                }
            }

            //给物体上的脚本使用了要注入值的特性注入，就是给那个属性注入
            SceneDependencyInjectContextManager.Instance.InjectsProperty(customComponent);
        }

        private void OnDestroy()
        {
            if (isDestroyUnRegister)
            {
                foreach (object item in useDynamicInjectBindComponent)
                {
                    var att = item.GetType().GetCustomAttributes(DynamicInjectBindAttributeType, false)[0] as DynamicInjectBindAttribute;

                    switch (att.BindingType)
                    {
                        case BindingType.BindingToSelf:
                            registerBinding.UnRegister(item.GetType());
                            break;
                        case BindingType.BindingToSelfAndAllInterface:
                            registerBinding.UnBindingToSelfAndAllInterface(item.GetType());
                            break;
                        case BindingType.BindingToAllInterface:
                            registerBinding.UnBindingToSelfAndAllInterface(item.GetType());
                            break;
                    }
                }
            }
        }
    }
}
