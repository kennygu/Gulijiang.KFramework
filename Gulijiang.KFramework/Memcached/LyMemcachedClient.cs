using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using Memcached.ClientLibrary;

namespace Gulijiang.KFramework.Memcached
{
    public class LyMemcachedClient
    {


        private static MemcachedClient cache = null;
        static LyMemcachedClient()
        {
            string[] servers = { ConfigurationManager.AppSettings["MemcacheServer"] };
            //初始化池
            SockIOPool pool = SockIOPool.GetInstance("LyCache");
            pool.SetServers(servers);
            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();
            MemcachedClient mc = new MemcachedClient();
            mc.EnableCompression = false;
        }

        #region Add 缓存添加,键值存在不能添加成功
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cache_key">缓存键</param>
        /// <param name="cache_object">缓存值</param>
        /// <returns></returns>
        public static bool Add(string cache_key, object cache_object)
        {
            cache_key = cache_key.Trim();
            return cache.Add(cache_key.ToUpper(), cache_object);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cache_key">缓存键</param>
        /// <param name="cache_object">缓存值</param>
        /// <param name="expiration">过期时间</param>
        /// <returns></returns>
        public static bool Add(string cache_key, object cache_object, DateTime expiration)
        {

            return cache.Add(cache_key.ToUpper(), cache_object, expiration);
        }

        #endregion

        #region Set缓存添加,键值不存在添加,键值存在修改
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cache_key">缓存键</param>
        /// <param name="cache_object">缓存值</param>
        /// <returns></returns>
        public static bool Set(string cache_key, object cache_object)
        {
            cache_key = cache_key.Trim();
            return cache.Set(cache_key.ToUpper(), cache_object);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cache_key">缓存键</param>
        /// <param name="cache_object">缓存值</param>
        /// <param name="expiration">缓存时长 秒数</param>
        /// <returns></returns>
        public static bool Set(string cache_key, object cache_object, int expLong)
        {
            cache_key = cache_key.Trim();
            return cache.Set(cache_key.ToUpper(), cache_object, DateTime.Now.AddSeconds(expLong));
        }

        #endregion



        #region  缓存获取
        /// <summary>
        /// 根据键获取缓存数据
        /// </summary>
        /// <param name="cache_key"></param>
        /// <returns></returns>
        public static object Get(string cache_key)
        {
            cache_key = cache_key.Trim();
            return cache.Get(cache_key.ToUpper());
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="cache_key">缓存键</param>
        /// <returns></returns>
        public static T Get<T>(string cache_key)
        {
            cache_key = cache_key.Trim();
            T entity = default(T);
            entity = (T)cache.Get(cache_key.ToUpper());
            return entity;
        }
        #endregion


        /// <summary>
        /// 根据键清空缓存
        /// </summary>
        /// <param name="cache_key"></param>
        public static bool Delete(string cache_key)
        {
            cache_key = cache_key.Trim();
            cache.Delete(cache_key.ToUpper());

            return false;
        }

        /// <summary>
        ///  清空所有缓存
        /// </summary>
        public static bool Flush()
        {
            return cache.FlushAll();
        }


    }

}

