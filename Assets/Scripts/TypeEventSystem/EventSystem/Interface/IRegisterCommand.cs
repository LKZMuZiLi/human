using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.TypeEventSystem
{

    /// <summary>
    /// 监听命令接口
    /// </summary>
    public interface IRegisterCommand
    {
        /// <summary>
        /// 监听
        /// 不需要监听事件的华，就调用UnRegister取消监听事件
        /// </summary>
        /// <param name="action">回调</param>
        public void Register<T>(Action<T> action)
            where T : struct;

        /// <summary>
        /// 取消监听事件
        /// </summary>
        /// <param name="action">取消监听的回调委托</param>
        public void UnRegister<T>(Action<T> action)
            where T : struct;
    }
}
