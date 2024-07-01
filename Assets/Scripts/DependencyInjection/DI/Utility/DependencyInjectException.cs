using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 依赖注入错误
    /// </summary>
  public  static   class DependencyInjectException
    {
        /// <summary>
        /// 抛出
        /// </summary>
        /// <param name="message"></param>
        public static void Throw(string message)
        {
            throw new Exception(message);
        }
    }
}
