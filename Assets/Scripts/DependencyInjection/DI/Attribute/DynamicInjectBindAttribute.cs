using System;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 动态的绑定
    /// 就是脚本没有跟着场景一起启动，而是这个脚本是后面使用代码添加上去的，就用到这个特性
    /// 说明这个脚本需要绑定注入到依赖注入中
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple= false, Inherited= false)]
    public class DynamicInjectBindAttribute : Attribute
    {
        /// <summary>
        /// 绑定的类型
        /// </summary>
        public BindingType BindingType;

        public DynamicInjectBindAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingType">绑定的类型</param>
        public DynamicInjectBindAttribute(BindingType bindingType)
        {
            BindingType = bindingType;
        }
    }
}
