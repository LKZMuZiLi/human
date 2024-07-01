namespace LKZ.DependencyInject
{
    /// <summary>
    /// 依赖注入，外面继承这个接口，就可以注入一个接口给你绑定的
    /// </summary>
    public  interface IDRegisterBindingInterface
    {
        /// <summary>
        /// 注册绑定
        /// 启动会自动调用这个方法
        /// </summary>
        /// <param name="registerBinding">注入绑定的接口，可以通过这个接口来绑定</param>
        void DIRegisterBinding(IRegisterBinding registerBinding);
    }
}
