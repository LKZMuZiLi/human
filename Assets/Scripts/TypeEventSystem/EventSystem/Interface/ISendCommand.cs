using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.TypeEventSystem
{
    /// <summary>
    /// 发送命令接口
    /// </summary>
    public interface ISendCommand
    {
        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="instanceParameter">这个是那个命令，就是注册的委托的类型
        /// 这个是一个实例，就是那个注册委托的参数结构体，结构体里面可以放给监听者传递参数</param>
        public void Send<T>(T instanceParameter)
            where T : struct;
        }
}
