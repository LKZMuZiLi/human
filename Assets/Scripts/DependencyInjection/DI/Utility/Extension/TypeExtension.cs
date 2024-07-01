using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject.Extension
{
    /// <summary>
    ///  Type扩展
    /// </summary>
    internal static  class TypeExtension
    {
        /// <summary>
        /// 需要过滤的命名空间的名字
        /// </summary>
        private readonly static string[] filterNamespaceName = new string[]
        {
            "UnityEngine",
            "System"
        };

        /// <summary>
        /// 需要过滤的程序集的名字
        /// </summary>
        private readonly static string[] filterAssemblyName = new string[]
        {
            "UnityEngine"
        };


        /// <summary>
        /// 依赖注入的特性类型
        /// </summary>
        private readonly static Type InjectAttributeType = typeof(InjectAttribute);

        /// <summary>
        /// 获得自定义接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns>返回那个自定义的接口数组</returns>
        internal static Type[] GetCustomInterface(this Type type)
        {
            Type[] allInterfaces = type.GetInterfaces();
            Type[] customInterface = allInterfaces.Where(item => IsNotContains(item.Namespace, filterNamespaceName)).ToArray();
            return customInterface;
        }

        /// <summary>
        /// 获得使用了inject特性的属性，这个属性是要注入内容的
        /// </summary>
        /// <param name="type"></param>
        /// <returns>返回一个迭代器</returns>
        internal static IEnumerable<PropertyInfo> GetUseInjectAttributeProperty(this Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Static| BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty|  BindingFlags.FlattenHierarchy);
            //这个DeclaredOnly是获得自己的，不获得父类的,我这里需要获得父类的，有可能那个类有继承，但是他不会获得父类的private属性
            //FlattenHierarchy  是获得父类的静态方法，不获得private方法
            PropertyInfo[] noContainUnityProperty = propertyInfos.Where(item => IsNotContains(item.Module.Name, filterAssemblyName)).ToArray();
          return   noContainUnityProperty.Where(item => item.IsDefined(InjectAttributeType, true));
        }

        /// <summary>
        /// 过滤掉别的东西
        /// </summary>
        /// <param name="filters">过滤的集合</param>
        /// <param name="judgeValue">需要判断的字符串</param>
        /// <returns>如果不包含返回true</returns>
        private static bool IsNotContains(string judgeValue, params string[] filters)
        {
            if (string.IsNullOrEmpty(judgeValue))
                return true;
            return !filters.Any(item => judgeValue.StartsWith(item));
        }

    }
}
