using LKZ.DependencyInject.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 依赖注入存储
    /// 上下文
    /// </summary>
    internal sealed class DependencyInjectContext : IRegisterBinding, IGetBindingImplementation
    {
        /// <summary>
        /// 存储所有的依赖注入的绑定类型和实现类型
        /// 键：就是那个绑定的类型，是哪个接口
        /// 值：是那个实现的接口的实体
        /// </summary>
        public Dictionary<Type, IDependencyInjectItem> DependencyInjectBind_Dic = new Dictionary<Type, IDependencyInjectItem>();

        /// <summary>
        /// 注入接口方法的参数
        /// </summary>
        public object[] RegisterBindingInterfaceParameter { get; }

        public DependencyInjectContext()
        {
            ///绑定自身所有的接口
            BindingToAllInterface(this);
            RegisterBindingInterfaceParameter = new object[] { this };
        }

        public BindingTo<TBindType> Binding<TBindType>()
        {
            Type bindType = typeof(TBindType);
            DependencyInjectItem<TBindType> dependencyInjectItem = new DependencyInjectItem<TBindType>();
            if (DependencyInjectBind_Dic.ContainsKey(bindType))
            {
                DependencyInjectException.Throw($"绑定的类型{bindType.Name}已经存在，不需要再次绑定");
            }
            else
            {
                DependencyInjectBind_Dic.Add(bindType, dependencyInjectItem);
            }
            return dependencyInjectItem;
        }

        public BindingTo Binding(Type bindType)
        {
            DependencyInjectItem<object> dependencyInjectItem = new DependencyInjectItem<object>();
            if (DependencyInjectBind_Dic.ContainsKey(bindType))
            {
                DependencyInjectException.Throw($"绑定的类型{bindType.Name}已经存在，不需要再次绑定");
            }
            else
            {
                DependencyInjectBind_Dic.Add(bindType, dependencyInjectItem);
            }
            return dependencyInjectItem;
        }


        public void BindingToAllInterface<TBindType>(TBindType instance)
            where TBindType : new()
        {
            Type[] allCustomInterface = instance.GetType().GetCustomInterface();
            foreach (Type item in allCustomInterface)
            {
                Binding(item).To(instance);
            }
        }

        public void BindingToSelf<TBindType>(TBindType instance)
        {
            Binding(instance.GetType()).To(instance);
        }

        public void BindingToSelfAndAllInterface<TBindType>(TBindType instance)
            where TBindType : new()
        {
            BindingToAllInterface(instance);
            BindingToSelf(instance);
        }

        public TBindType GetBindType<TBindType>()
        {
            Type type = typeof(TBindType);
            return (TBindType)GetBindType(type);
        }

        public object GetBindType(Type bindType)
        {
            object obj = null;
            if (DependencyInjectBind_Dic.TryGetValue(bindType, out IDependencyInjectItem value))
            {
#if UNITY_EDITOR
                if (!bindType.IsAssignableFrom(value.BindInstance.GetType()))
                {
                    DependencyInjectException.Throw($"强转失败，{bindType.FullName}类型强转不了{value.BindInstance.GetType().FullName}");
                }
#endif
                obj = value.BindInstance;
            }

#if UNITY_EDITOR
            if (obj == null)
            {
                DependencyInjectException.Throw($"类型没有注册{bindType.FullName}");
            }
#endif
            return obj;
        }

        public void UnRegister<TBindType>()
        {
            this.UnRegister(typeof(TBindType));
        }

        public void UnRegister(Type bindType)
        {
            bool isSucceed = DependencyInjectBind_Dic.Remove(bindType);
#if UNITY_EDITOR
            if (!isSucceed)
            {
                DependencyInjectException.Throw($"取消注册失败，{bindType.FullName}这个没有绑定");
            }
#endif
        }

        public void UnBindingToSelfAndAllInterface(Type bindType)
        {
            this.UnBindingToAllInterface(bindType);
            UnRegister(bindType);//取消本身
        }

        public void UnBindingToAllInterface(Type bindType)
        {
            Type[] allCustomInterface = bindType.GetCustomInterface();
            foreach (Type item in allCustomInterface)
            {
                UnRegister(item);
            }
        }
    }
}
