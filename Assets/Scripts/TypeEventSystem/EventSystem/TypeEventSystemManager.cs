using LKZ.DependencyInject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.TypeEventSystem
{
    /// <summary>
    /// 类型事件系统管理
    /// </summary>
    [DefaultExecutionOrder(-100)]
    [HelpURL("http://www.lkz.fit")]
    [DisallowMultipleComponent]
    [AddComponentMenu("LKZ/类型事件系统管理")]
    public class TypeEventSystemManager : MonoBehaviour, ISendCommand, IRegisterCommand, IDRegisterBindingInterface 
    {
        [SerializeField, Tooltip("是否加载场景不销毁")]
        private bool dontDestroyOnLoad = true;

        /// <summary>
        /// 存储注册的接口的集合
        /// 键是那个注册的类型，是一个结构体，等于是一个命令
        /// 值是那个键的委托返回值的接口，就是EventRegister<T>这个类
        /// </summary>
        private Dictionary<Type, IEventInterface> allEventType_Dic;

         
        #region 公开 接口方法
        /// <summary>
        /// 监听
        /// 不需要监听事件的华，就调用UnRegister取消监听事件
        /// </summary>
        /// <param name="action">回调</param>
        public void Register<T>(Action<T> action)
            where T : struct
        {
            Type key = typeof(T);
            if (allEventType_Dic.TryGetValue(key, out IEventInterface value))
            {
                (value as EventRegister<T>).Register(action);
            }
            else
            {
                allEventType_Dic.Add(key, new EventRegister<T>(action));
            }
        }

        /// <summary>
        /// 取消监听事件
        /// </summary>
        /// <param name="action">取消监听的回调委托</param>
        public void UnRegister<T>(Action<T> action)
            where T : struct
        {
            Type key = typeof(T);
            if (allEventType_Dic.TryGetValue(key, out IEventInterface value))
            {
                (value as EventRegister<T>).UnRegister(action);
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="instanceParameter">这个是那个命令，就是注册的委托的类型
        /// 这个是一个实例，就是那个注册委托的参数结构体，结构体里面可以放给监听者传递参数</param>
        public void Send<T>(T instanceParameter)
            where T : struct
        {
            Type key = typeof(T);
            if (allEventType_Dic.TryGetValue(key, out IEventInterface value))
            {
                (value as EventRegister<T>).Send(instanceParameter);
            }
        }
        #endregion

        #region 依赖注入接口方法
        /// <summary>
        /// 依赖注入回调
        /// 我把自己的接口注入进去
        /// </summary>
        /// <param name="registerBinding"></param>
        void IDRegisterBindingInterface.DIRegisterBinding(IRegisterBinding registerBinding)
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this.gameObject);
            allEventType_Dic = new Dictionary<Type, IEventInterface>();

#if UNITY_EDITOR
            if (FindObjectsOfType<TypeEventSystemManager>().Length > 1)
                Debug.LogError($"场景中有多个{nameof(TypeEventSystemManager)}脚本!");
#endif

            registerBinding.Binding<ISendCommand>().To(this);
            registerBinding.Binding<IRegisterCommand>().To(this);
        }

        #endregion

    }
}