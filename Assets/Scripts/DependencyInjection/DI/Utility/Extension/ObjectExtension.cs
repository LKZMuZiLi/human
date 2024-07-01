using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 获得
    /// </summary>
   internal static  class ObjectExtension
    {
        /// <summary>
        /// 动态的绑定特性标识类型
        /// </summary>
        private readonly static Type DynamicInjectBindAttributeType = typeof(DynamicInjectBindAttribute);

        /// <summary>
        /// 获得使用了DynamicInjectBindAttribute特性的类
        /// </summary>
        /// <param name="objs">需要判断的类集合</param>
        /// <returns>返回获得使用这个特性的迭代器</returns>
        public static object[] GetUseDynamicInjectBindAttribute(this object[] objs)
        {
           return  objs.Where(item => item.GetType().IsDefined(DynamicInjectBindAttributeType, false)).ToArray();
        }
    }
}
