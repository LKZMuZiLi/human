using LKZ.DependencyInject.Extension;
using UnityEngine;
using UnityEngine.Serialization;

namespace LKZ.DependencyInject
{
    /// <summary>
    /// 场景的依赖注入上下文
    /// </summary>
    [DefaultExecutionOrder(-200)]
    [HelpURL("http://www.lkz.fit")]
    [DisallowMultipleComponent]
    [AddComponentMenu("LKZ/依赖注入/依赖注入场景上下文管理")]
    public class SceneDependencyInjectContextManager : MonoBehaviour
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static SceneDependencyInjectContextManager Instance { get; private set; }

        [SerializeField, FormerlySerializedAs("是否加载场景不销毁")]
        private bool dontDestroyOnLoad = true;

        /// <summary>
        /// 依赖注入Unity配置文件
        /// </summary>
        [SerializeField, FormerlySerializedAs("依赖注入Unity配置文件")]
        private DIScriptableObject[] scriptableObjects;

        /// <summary>
        /// 依赖注入上下文
        /// </summary>
        private DependencyInjectContext dependencyInjectContext;

        private object[] AllCustomComponent;

        private void Awake()
        {
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this.gameObject);

            Instance = this;
#if UNITY_EDITOR
            if (FindObjectsOfType<SceneDependencyInjectContextManager>().Length > 1)
                Debug.LogError($"场景中有多个{nameof(SceneDependencyInjectContextManager)}脚本!");
#endif

            dependencyInjectContext = new DependencyInjectContext();
            //获得自定义组件
              AllCustomComponent = dependencyInjectContext.GetCustomComponent();

            //调用继承了注入接口IDIRegisterBinding方法
            InvokeBindInjectsInterface(AllCustomComponent);
            //调用Unity配置文件的注入绑定
            InvokeInjectBindingScriptableObject();

            #region 给使用了Inject特性的属性注入

            InjectsProperty(AllCustomComponent);
            #endregion

            dependencyInjectContext.InvokeDIAwakeInterface(AllCustomComponent);
        }

        private void Start()
        {
            dependencyInjectContext.InvokeDIStartInterface(AllCustomComponent);
        }

        #region 调用Unity配置文件的注入绑定 私有
        /// <summary>
        /// 调用Unity配置文件的注入绑定
        /// </summary>
        private void InvokeInjectBindingScriptableObject()
        {
            foreach (DIScriptableObject item in scriptableObjects)
            {
                item.InjectBinding(dependencyInjectContext);
            }
        }
        #endregion
         

        #region 调用注入绑定的接口
        /// <summary>
        /// 调用注入绑定的接口
        /// 场景中有谁继承了IDIRegisterBinding接口，就调用那个接口方法
        /// </summary>
        /// <param name="component">自定义的组件</param>
        public void InvokeBindInjectInterface(object component)
        {
            dependencyInjectContext.InvokeBindInjectInterface(component);
        }

        /// <summary>
        /// 调用注入绑定的接口
        /// 场景中有谁继承了IDIRegisterBinding接口，就调用那个接口方法
        /// </summary>
        /// <param name="component">自定义的组件</param>
        public void InvokeBindInjectsInterface(params object[] component)
        {
            //调用继承了注入接口IDIRegisterBinding方法
            dependencyInjectContext.InvokeBindInjectInterface(component);
        }
        #endregion

        #region 注入属性值
        /// <summary>
        /// 注入属性值
        /// </summary>
        /// <param name="component">组件的实例</param>
        public void InjectProperty(object component)
        {
            foreach (var property in component.GetType().GetUseInjectAttributeProperty())
            {
                dependencyInjectContext.InjectProperty(component, property);
            }
        }

        /// <summary>
        /// 注入属性值
        /// </summary>
        /// <param name="component">组件的实例,多个组件</param>
        public void InjectsProperty(object[] components)
        {
            foreach (object item in components)
            {
                foreach (var property in item.GetType().GetUseInjectAttributeProperty())
                {
                    dependencyInjectContext.InjectProperty(item, property);
                }
            }
            
        }
        #endregion

        #region 获得绑定依赖注册的接口
        /// <summary>
        /// 获得绑定依赖注册的接口
        /// </summary>
        /// <returns></returns>
        public IRegisterBinding GetRegisterBindingInterface()
        {
            return dependencyInjectContext;
        }
        #endregion

    }

}