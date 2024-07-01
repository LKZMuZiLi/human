using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 自动绑定的Item
    /// </summary>
    [Serializable]
    public struct AutoBindingItemStruct
    {
        /// <summary>
        /// 绑定类型的操作
        /// </summary>
        [FormerlySerializedAs("绑定类型的操作")]
        public BindingType bindingType;

        /// <summary>
        /// 绑定类型的操作
        /// </summary>
        [FormerlySerializedAs("绑定实现的实现类组件")]
        public MonoBehaviour monoBehaviour;
    }
}
