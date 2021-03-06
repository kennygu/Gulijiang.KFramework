﻿using Castle.Core.Logging;

namespace Gulijiang.KFramework.Castle.Service
{
    /// <summary>
    /// 容器控制器
    /// </summary>
 //  [Transient]
    public class ContainerService 
    {
        private ILogger _logger = NullLogger.Instance;
        /// <summary>
        /// 日志对象
        /// </summary>
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }


        /// <summary>
        /// 获取Persistence
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        protected T GetPersistence<T>() where T : class
        {
            return CastleIocHelper.Container.Resolve<T>();
        }

        /// <summary>
        /// 获取Persistence
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        protected object GetPersistence(string key)
        {
            if (CastleIocHelper.Container.Kernel.HasComponent(key))
            {
                return CastleIocHelper.Container[key];
            }
            return null;

        }
    }
}