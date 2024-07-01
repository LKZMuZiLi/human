using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 注册到哪一个的
    /// </summary>
    public interface BindingTo<in  T>
    {
        /// <summary>
        /// 绑定到那个实现类实现
        /// </summary>
        /// <typeparam name="Instance">实现类的类型</typeparam>
        /// <param name="instance">那个实现类的实例</param>
        void To<Instance>(Instance instance)
            where Instance : T;
    }

    /// <summary>
    /// 注册到哪一个的
    /// </summary>
    public interface BindingTo
    {
        /// <summary>
        /// 绑定到那个实现类实现
        /// </summary>
        /// <param name="instance">那个实现类的实例</param>
        void To(object instance);
    }
}
