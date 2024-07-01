using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 依赖注入的Item每一个元素
    /// </summary>
    /// <typeparam name="T">这个是注册了那个类型</typeparam>
    internal sealed class DependencyInjectItem<T> : BindingTo<T>, BindingTo, IDependencyInjectItem
    {
        /// <summary>
        /// 绑定的那个实现类
        /// </summary>
      public   object BindInstance { get;set; }

        public void To<Instance>(Instance instance) where Instance : T
        {
            BindInstance = instance;
        }

        public void To(object instance)
        {
            BindInstance = instance;
        }
    }
}
