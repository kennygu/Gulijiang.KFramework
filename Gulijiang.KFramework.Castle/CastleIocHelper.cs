using System;
using Castle.Windsor;

namespace Gulijiang.KFramework.Castle
{
    public static class CastleIocHelper
    {
        private static IWindsorContainer _container;

        /// <summary>
        /// The inner Windsor container.
        /// </summary>
        public static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    throw new InvalidOperationException("Container 未初始化!");
                }
                return _container;
            }
        }

        /// <summary>
        /// Indicates if the container is initialized properly.
        /// </summary>
        public static bool IsInitialized
        {
            get { return _container != null; }
        }

        /// <summary>
        /// Initialize the IoC wrapper
        /// </summary>
        /// <param name="windsorContainer"></param>
        public static void Initialize(IWindsorContainer windsorContainer)
        {
            _container = windsorContainer;
        }

        /// <summary>
        /// Resolve a service of the given type.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static object Resolve(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        /// <summary>
        /// Resolve a service of the given type and with the given name.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static object Resolve(Type serviceType, string serviceName)
        {
            return Container.Resolve(serviceName, serviceType);
        }

        /// <summary>
        /// Resolve a service of the given type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            try
            {
                var tObj=Container.Resolve<T>();
                return tObj;
            }
            catch (Exception e)
            {
                throw new Exception("从容器获取类型失败："+typeof(T).ToString()+e.Message);
            }
             
        }

        /// <summary>
        /// Resolve a service of the given type parameter and with the given name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }

        /// <summary>
        /// Check if a component of type T is registered in the container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasComponent<T>()
        {
            return Container.Kernel.HasComponent(typeof(T));
        }
    }
}
