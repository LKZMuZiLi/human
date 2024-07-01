using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 注册绑定接口的
    /// </summary>
    public  interface IRegisterBinding
    {
        /// <summary>
        /// 注册绑定的
        /// </summary>
        /// <typeparam name="TBindType">绑定什么类型</typeparam>
        /// <returns>返回那个绑定后的接口，可以调用To绑定哪一个实例</returns>
        BindingTo<TBindType> Binding<TBindType>();

        /// <summary>
        /// 注册绑定的
        /// </summary>
        /// <param name="bindType">绑定什么类型</param>
        /// <returns>返回那个绑定后的接口，可以调用To绑定哪一个实例</returns>
        BindingTo Binding(Type bindType);

        /// <summary>
        /// 注册绑定的To也是自身的
        /// </summary>
        /// <typeparam name="TBindType">绑定什么实现类型</typeparam>
        /// <param name="instance">实现的那个实例</param>
        void BindingToSelf<TBindType>(TBindType instance);

        /// <summary>
        /// 注册绑定的To也是自身的继承所有的接口包括父类的，不包含Unity的接口和C#的接口
        /// </summary>
        /// <typeparam name="TBindType">绑定什么实现类型</typeparam>
        /// <param name="instance">实现的那个实例</param>
        void BindingToSelfAndAllInterface<TBindType>(TBindType instance) where TBindType : new();


        /// <summary>
        /// 注册绑定的To继承所有的接口包括父类的，不包含Unity的接口和C#的接口
        /// </summary>
        /// <typeparam name="TBindType">绑定什么实现类型</typeparam>
        /// <param name="instance">实现的那个实例</param>
        void BindingToAllInterface<TBindType>(TBindType instance)
            where TBindType : new();

        /// <summary>
        /// 取消注册绑定的
        /// </summary>
        /// <typeparam name="TBindType">取消注册的类型</typeparam>
        void UnRegister<TBindType>();

        /// <summary>
        /// 取消注册绑定的
        /// </summary>
        /// <param name="bindType">取消注册的类型</param>
        void UnRegister(Type bindType);

        /// <summary>
        /// 取消注册绑定的
        /// 取消绑定自身和自身或者父类的接口
        /// </summary>
        /// <param name="bindType">取消注册的类型，这个是那个实例</param>
        void UnBindingToSelfAndAllInterface(Type bindType);


        /// <summary>
        /// 取消注册绑定的
        /// 取消绑定自身或者父类的接口
        /// </summary>
        /// <param name="bindType">取消注册的类型，这个是那个实例</param>
        void UnBindingToAllInterface(Type bindType);
    }
}
