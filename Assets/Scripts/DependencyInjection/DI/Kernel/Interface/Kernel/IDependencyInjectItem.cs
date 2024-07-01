using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 依赖注入接口
    /// </summary>
   internal interface IDependencyInjectItem
    {
        /// <summary>
        /// 绑定的那个实现类
        /// </summary>
         object BindInstance { get;set; }
    }
}
