using System;

namespace LKZ.TypeEventSystem
{
    /// <summary>
    /// 事件监听
    /// 实现存储回调的委托的
    /// </summary>
    /// <typeparam name="T">回调委托的参数</typeparam>
    internal class EventRegister<T> : IEventInterface
    where T : struct
    {
        /// <summary>
        /// 回调委托事件
        /// </summary>
        private event Action<T> onEvent;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="callback">回调委托</param>
        public EventRegister(Action<T> callback)
        {
            onEvent += callback;
        }

        /// <summary>
        /// 监听
        /// 不需要监听事件的华，就调用UnRegister取消监听事件
        /// </summary>
        /// <param name="actionCallback">回调</param>
        public void Register(Action<T> actionCallback)
        {
            this.onEvent += actionCallback;
        }

        /// <summary>
        /// 取消监听事件
        /// </summary>
        /// <param name="actionCallback">取消监听的回调委托</param>
        public void UnRegister(Action<T> actionCallback)
        {
            this.onEvent -= actionCallback;
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="instanceParameter">这个是那个命令，就是注册的委托的类型
        /// 这个是一个实例，就是那个注册委托的参数结构体，结构体里面可以放给监听者传递参数</param>
        public void Send(T instanceParameter)
        {
            this.onEvent?.Invoke(instanceParameter);
        }
    }
}
