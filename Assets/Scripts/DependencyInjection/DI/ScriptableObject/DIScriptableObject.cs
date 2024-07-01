using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// Unity的配置文件
    /// 要继承这个类，才能注入
    /// </summary>
    public abstract class DIScriptableObject : ScriptableObject
    {
        /// <summary>
        /// 启动的时候会自动调用这个方法
        /// </summary>
        /// <param name="registerBinding">绑定的依赖注入的接口</param>
        public abstract void InjectBinding(IRegisterBinding registerBinding);
    }
}
