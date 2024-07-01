using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 自动依赖注入绑定
    /// 这个要跟着场景一起启动的
    /// 如果想场景启动了，动态给场景添加脚本自动注入
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("LKZ/依赖注入/自动绑定依赖注入",order:11)]
    public class AutoInjectDIBind : MonoBehaviour, IDRegisterBindingInterface
    {
        /// <summary>
        /// 需要绑定的集合
        /// </summary>
        [SerializeField,  FormerlySerializedAs("需要绑定的集合")]
        private AutoBindingItemStruct[] autoBindingItemStructs;

        /// <summary>
        /// 是否销毁了取消注册
        /// </summary>
        [SerializeField, FormerlySerializedAs("是否销毁了取消注册")]
        private bool isDestroyUnRegister = true;

        /// <summary>
        /// 注入的引用
        /// </summary>
        private IRegisterBinding registerBinding;

        /// <summary>
        /// 自动时自动调用这个绑定
        /// </summary>
        /// <param name="registerBinding"></param>
        void IDRegisterBindingInterface.DIRegisterBinding(IRegisterBinding registerBinding)
        {
            this.registerBinding = registerBinding;
            foreach (AutoBindingItemStruct item in autoBindingItemStructs)
            {
                switch (item.bindingType)
                {
                    case BindingType.BindingToSelf:
                        registerBinding.BindingToSelf(item.monoBehaviour);
                        break;
                    case BindingType.BindingToSelfAndAllInterface:
                        registerBinding.BindingToSelfAndAllInterface(item.monoBehaviour);
                        break;
                    case BindingType.BindingToAllInterface:
                        registerBinding.BindingToAllInterface(item.monoBehaviour);
                        break;
                }
            }
        }

        private void OnDestroy()
        {
            if (isDestroyUnRegister)
            {
                foreach (AutoBindingItemStruct item in autoBindingItemStructs)
                {
                    switch (item.bindingType)
                    {
                        case BindingType.BindingToSelf:
                            registerBinding.UnRegister(item.monoBehaviour.GetType());
                            break;
                        case BindingType.BindingToSelfAndAllInterface:
                            registerBinding.UnBindingToSelfAndAllInterface(item.monoBehaviour.GetType());
                            break;
                        case BindingType.BindingToAllInterface:
                            registerBinding.UnBindingToAllInterface(item.monoBehaviour.GetType());
                            break;
                    }
                }
            }
        }
    }
}
