using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 获得绑定的实现类型
    /// </summary>
   public interface IGetBindingImplementation
    {
        /// <summary>
        /// 获得绑定的类型
        /// </summary>
        /// <typeparam name="TBindType">绑定的类型</typeparam>
        /// <returns>返回那个绑定的接口实现</returns>
        TBindType GetBindType<TBindType>();

        /// <summary>
        /// 获得绑定的接口吧
        /// </summary>
        /// <param name="bindType">绑定的类型</param>
        /// <returns>返回那个绑定的接口实现</returns>
        object GetBindType(Type bindType);
    }
}
