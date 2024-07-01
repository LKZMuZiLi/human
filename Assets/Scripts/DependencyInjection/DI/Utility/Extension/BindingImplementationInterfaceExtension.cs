using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// IGetBindingImplementation 获得注册的实现类型的接口扩展
    /// </summary>
    internal static class BindingImplementationInterfaceExtension
    {
        /// <summary>
        /// 注入属性值
        /// </summary>
        /// <param name="getBindingImplementation"></param>
        /// <param name="component">组件的实例</param>
        /// <param name="propertyInfo">那个属性的引用</param>
        internal static void InjectProperty(this IGetBindingImplementation getBindingImplementation, object component, PropertyInfo propertyInfo)
        {
            if (component == null)
                DependencyInjectException.Throw("注入失败，实例为空");
            if (propertyInfo == null)
                DependencyInjectException.Throw("注入失败，注入的方法为空");

            //获得绑定注册的实现类型
            object obj = getBindingImplementation.GetBindType(propertyInfo.PropertyType);

            //设置值
            propertyInfo.SetValue(component, obj);
        }
    }
}
