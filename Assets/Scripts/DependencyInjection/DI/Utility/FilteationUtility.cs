using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 过滤的工具
    /// </summary>
   public static  class FilteationUtility
    {
        /// <summary>
        /// 需要过滤的程序集名字
        /// </summary>
        private static string[] filtrationAssemblyName = new string[]
        {
            "UnityEngine"
        };

        /// <summary>
        /// 过滤调Unity自己的组件
        /// </summary>
        /// <param name="allComponent"></param>
        /// <returns></returns>
        public static object[] FiltrationComponent(object[] allComponent)
        {
            return allComponent.Where(item => 
                 IsNotContainsAssemblyName(item.GetType().FullName)).ToArray();
        }

        /// <summary>
        /// 过滤掉别的命名空间
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsNotContainsAssemblyName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return true;

            return !filtrationAssemblyName.Any(item => name.Contains(item));
        }
    }
}
