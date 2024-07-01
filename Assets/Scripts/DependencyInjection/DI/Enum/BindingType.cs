using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 绑定的类型
    /// </summary>
    public enum BindingType
    {
        /// <summary>
        /// 就绑定自己的类和实现类
        /// </summary>
        [InspectorName("就绑定自己的类和实现类")]
        BindingToSelf,

        /// <summary>
        /// 绑定类的所有的接口，包含类的本身
        /// </summary>
        [InspectorName("绑定类的所有的接口，包含类的本身")]
        BindingToSelfAndAllInterface,

        /// <summary>
        /// 绑定类的所有的接口，不包含类的本身
        /// </summary>
        [InspectorName("绑定类的所有的接口，不包含类的本身")]
        BindingToAllInterface
    }

}