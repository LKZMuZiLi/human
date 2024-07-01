using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// DependencyInjectContext的扩展
    /// </summary>
    internal static  class DependencyInjectContextExtension
    {
        /// <summary>
        /// DI注入接口名字
        /// </summary>
        private static readonly string  IDRegisterBindingName = nameof(IDRegisterBindingInterface);
        /// <summary>
        /// DI注入接口名字的方法名字
        /// </summary>
        private static readonly string IDRegisterBindingMethodName = nameof(IDRegisterBindingInterface.DIRegisterBinding);


        /// <summary>
        /// DIawake接口
        /// </summary>
        private static readonly string DIAwakeInterfaceName = nameof(DIAwakeInterface);
        /// <summary>
        /// DIAwake注入的方法名字
        /// </summary>
        private static readonly string DIAwakeInterfaceMethodName = nameof(DIAwakeInterface.OnAwake);

        /// <summary>
        /// DIStart接口
        /// </summary>
        private static readonly string DIStartInterfaceName = nameof(DIStartInterface);
        /// <summary>
        /// DIStart注入的方法名字
        /// </summary>
        private static readonly string DIStartInterfaceMethodName = nameof(DIStartInterface.OnStart);

        /// <summary>
        /// 反射获得方法过滤
        /// </summary>
        private static readonly BindingFlags GetMethodBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod;

        #region 获得自定义组件
        /// <summary>
        /// 获得自定义组件
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        internal static object[] GetCustomComponent(this DependencyInjectContext dependencyInjectContext)
        {
            object[] allComponent = Object.FindObjectsOfType(typeof(MonoBehaviour),true);
            return FilteationUtility.FiltrationComponent(allComponent.Select(item => item).ToArray());
        }
        #endregion

        /// <summary>
        /// 调用注入的接口
        /// IDIRegisterBinding就是这个
        /// </summary>
        /// <param name="dependencyInjectContext"></param>
        /// <param name="componentInstance">要调用的接口实例</param>
        internal static void InvokeBindInjectInterface(this DependencyInjectContext dependencyInjectContext,object componentInstance)
        {
            Type registerInterface = componentInstance.GetType().GetInterface(IDRegisterBindingName);
            if (registerInterface != null)
            {
                MethodInfo registerMethod = registerInterface.GetMethod(IDRegisterBindingMethodName, GetMethodBindingFlags);
                if (registerMethod != null)
                {
                    registerMethod.Invoke(componentInstance,dependencyInjectContext.RegisterBindingInterfaceParameter);
                }
            }
        }

        /// <summary>
        /// 调用注入的接口
        /// 场景中有谁继承了IDIRegisterBinding接口，就调用那个接口方法
        /// </summary>
        /// <param name="dependencyInjectContext"></param>
        /// <param name="componentInstance">自定义的组件</param>
        internal static void InvokeBindInjectInterface(this DependencyInjectContext dependencyInjectContext,params object[] componentInstance)
        {
            foreach (object item in componentInstance)
            {
                InvokeBindInjectInterface(dependencyInjectContext, item);
            }
        }

        /// <summary>
        /// 调用DIawake接口方法
        /// </summary>
        /// <param name="dependencyInjectContext"></param>
        /// <param name="componentInstances">自定义的组件</param>
        internal static void InvokeDIAwakeInterface(this DependencyInjectContext dependencyInjectContext,object[] componentInstances)
        {
            for (int i = 0; i < componentInstances.Length; i++)
            {
                object temp = componentInstances[i];
                Type registerInterface = temp.GetType().GetInterface(DIAwakeInterfaceName);
                if (registerInterface != null)
                {
                    MethodInfo registerMethod = registerInterface.GetMethod(DIAwakeInterfaceMethodName, GetMethodBindingFlags);
                    if (registerMethod != null)
                    {
                        registerMethod.Invoke(temp, null);
                    }
                }
            } 
        }

        /// <summary>
        /// 调用DIStart接口方法
        /// </summary>
        /// <param name="dependencyInjectContext"></param>
        /// <param name="componentInstances">自定义的组件</param>
        internal static void InvokeDIStartInterface(this DependencyInjectContext dependencyInjectContext, object[] componentInstances)
        {
            for (int i = 0; i < componentInstances.Length; i++)
            {
                object temp = componentInstances[i];
                Type registerInterface = temp.GetType().GetInterface(DIStartInterfaceName);
                if (registerInterface != null)
                {
                    MethodInfo registerMethod = registerInterface.GetMethod(DIStartInterfaceMethodName, GetMethodBindingFlags);
                    if (registerMethod != null)
                    {
                        registerMethod.Invoke(temp, null);
                    }
                }
            }
        }

    }
}
