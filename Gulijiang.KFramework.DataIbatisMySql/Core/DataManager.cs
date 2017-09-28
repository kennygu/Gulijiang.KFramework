using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.SessionStore;

namespace Gulijiang.KFramework.DataIbatisMySql.Core
{
    /// <summary>
    /// 数据操作对象
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// 写库操作对象
        /// </summary>
        public ISqlMapper DBOperator;
        /// <summary>
        /// 锁定对象
        /// </summary>
        private static readonly object syncObj = new object();
        /// <summary>
        /// 单例从配置文件构造数据操作对象
        /// </summary>
        public DataManager(string dbTagName)
        {
            if (DBOperator == null)
            {
                lock (syncObj)
                {
                    if (DBOperator == null)
                    {
                        try
                        {
                            string strBatisConfig = ConfigurationManager.AppSettings["DBAssembly"];
                            string[] batisConfigArray = strBatisConfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            Assembly assembly = Assembly.Load(batisConfigArray[0]);
                            Stream stream = assembly.GetManifestResourceStream(batisConfigArray[0] + "." + batisConfigArray[1]);
                            DomSqlMapBuilder builder = new DomSqlMapBuilder();
                            DBOperator = builder.Configure(stream);
                            DBOperator.DataSource.ConnectionString = ConfigurationManager.ConnectionStrings[dbTagName].ConnectionString;
                            DBOperator.SessionStore = new HybridWebThreadSessionStore(DBOperator.Id);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 单例从配置文件构造数据操作对象----castle控制事务时这边需要用castleIOC中的ISqlMapper传入才能控制事务
        /// </summary>
        public DataManager(object dbOperator, string dbTagName = "")
        {
            if (DBOperator == null)
            {
                //如果没有传ISqlMapper并且有传dbTagName则还是去访问XML
                if (dbOperator == null && dbTagName.Length > 0)
                {
                    try
                    {
                        string strBatisConfig = ConfigurationManager.AppSettings["DBAssembly"];
                        string[] batisConfigArray = strBatisConfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        Assembly assembly = Assembly.Load(batisConfigArray[0]);
                        Stream stream = assembly.GetManifestResourceStream(batisConfigArray[0] + "." + batisConfigArray[1]);
                        DomSqlMapBuilder builder = new DomSqlMapBuilder();
                        DBOperator = builder.Configure(stream);
                        DBOperator.DataSource.ConnectionString = ConfigurationManager.ConnectionStrings[dbTagName].ConnectionString;
                        DBOperator.SessionStore = new HybridWebThreadSessionStore(DBOperator.Id);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    DBOperator = dbOperator as ISqlMapper;
                }
            }
        }
        /// <summary>
        /// 重载返回object
        /// </summary>
        /// <typeparam name="T">对象1</typeparam>
        /// <param name="tag">节点</param>
        /// <param name="parms">参数</param>
        /// <returns>list</returns>
        public T QueryForObject<T>(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("数据库出错");
                var result = DBOperator.QueryForObject<T>(tag, parms);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="tag">SQL对象</param>
        /// <param name="parms">参数</param>
        /// <returns>返回状态</returns>
        public int ExecuteSql(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("数据库出错");
                var result = DBOperator.QueryForObject<int>(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        ///插入SQL
        /// </summary>
        /// <param name="tag">SQL对象</param>
        /// <param name="parms">参数</param>
        /// <returns>返回状态</returns>
        public object Insert(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("数据库出错");
                var result = DBOperator.Insert(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        ///更新SQL
        /// </summary>
        /// <param name="tag">SQL对象</param>
        /// <param name="parms">参数</param>
        /// <returns>返回状态</returns>
        public int Update(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("数据库出错");
                var result = DBOperator.Update(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        ///删除SQL
        /// </summary>
        /// <param name="tag">SQL对象</param>
        /// <param name="parms">参数</param>
        /// <returns>返回状态</returns>
        public int Delete(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("数据库出错");
                var result = DBOperator.Delete(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 重载返回list
        /// </summary>
        /// <typeparam name="T">对象1</typeparam>
        /// <param name="tag">节点</param>
        /// <param name="parms">参数</param>
        /// <returns>list</returns>
        public List<T> QueryForList<T>(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("系统出错!");
                IList<T> result = DBOperator.QueryForList<T>(tag, parms);
                List<T> res = null;
                if (result != null)
                    res = result as List<T>;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 重载返回list
        /// </summary>
        /// <typeparam name="T">对象1</typeparam>
        /// <param name="tag">节点</param>
        /// <param name="paramObject"></param>
        /// <param name="orderby"></param>
        /// <param name="beginNo"></param>
        /// <param name="endNo"></param>
        /// <param name="totalCount"></param>
        /// <returns>list</returns>
        public List<T> QueryForListWithPage<T>(string tag, object paramObject, string orderby, int beginNo, int endNo, ref int totalCount)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("系统出错!");
                IList<T> result = DBOperator.QueryForListWithPage<T>(tag, paramObject, orderby, beginNo, endNo, ref totalCount);
                List<T> res = null;
                if (result != null)
                    res = result as List<T>;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
