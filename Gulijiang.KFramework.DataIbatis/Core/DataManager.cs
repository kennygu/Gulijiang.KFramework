using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.SessionStore;

namespace Gulijiang.KFramework.DataIbatis.Core
{
    /// <summary>
    /// ���ݲ�������
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// д���������
        /// </summary>
        public ISqlMapper DBOperator;
        /// <summary>
        /// ��������
        /// </summary>
        private static readonly object syncObj = new object();
        /// <summary>
        /// �����������ļ��������ݲ�������
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
                            //DBOperator=(CastleIBatisContainer.GetContainer())["sqlServerSqlMap"] as ISqlMapper;
                            string strBatisConfig = ConfigurationManager.AppSettings["DBAssembly"];
                            string[] batisConfigArray = strBatisConfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            Assembly assembly = Assembly.Load(batisConfigArray[0]);
                            Stream stream = assembly.GetManifestResourceStream(batisConfigArray[0] + "." + batisConfigArray[1]);
                            DomSqlMapBuilder builder = new DomSqlMapBuilder();
                            DBOperator = builder.Configure(stream) as SqlMapper;
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
        /// �����������ļ��������ݲ�������----castle��������ʱ�����Ҫ��castleIOC�е�ISqlMapper������ܿ�������
        /// </summary>
        public DataManager(ISqlMapper dbOperator, string dbTagName = "")
        {
            if (DBOperator == null)
            {
                //���û�д�ISqlMapper�����д�dbTagName����ȥ����XML
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
                DBOperator = dbOperator;
            }
        }
        /// <summary>
        /// ���ط���object
        /// </summary>
        /// <typeparam name="T">����1</typeparam>
        /// <param name="tag">�ڵ�</param>
        /// <param name="parms">����</param>
        /// <returns>list</returns>
        public T QueryForObject<T>(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("���ݿ����");
                var result = DBOperator.QueryForObject<T>(tag, parms);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// ִ��SQL
        /// </summary>
        /// <param name="tag">SQL����</param>
        /// <param name="parms">����</param>
        /// <returns>����״̬</returns>
        public int ExecuteSql(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("���ݿ����");
                var result = DBOperator.QueryForObject<int>(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        ///����SQL
        /// </summary>
        /// <param name="tag">SQL����</param>
        /// <param name="parms">����</param>
        /// <returns>����״̬</returns>
        public object Insert(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("���ݿ����");
                var result = DBOperator.Insert(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        ///����SQL
        /// </summary>
        /// <param name="tag">SQL����</param>
        /// <param name="parms">����</param>
        /// <returns>����״̬</returns>
        public int Update(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("���ݿ����");
                var result = DBOperator.Update(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        ///ɾ��SQL
        /// </summary>
        /// <param name="tag">SQL����</param>
        /// <param name="parms">����</param>
        /// <returns>����״̬</returns>
        public int Delete(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("���ݿ����");
                var result = DBOperator.Delete(tag, parms);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// ���ط���list
        /// </summary>
        /// <typeparam name="T">����1</typeparam>
        /// <param name="tag">�ڵ�</param>
        /// <param name="parms">����</param>
        /// <returns>list</returns>
        public List<T> QueryForList<T>(string tag, object parms)
        {
            try
            {
                if (DBOperator == null)
                    throw new Exception("ϵͳ����!");
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
        /// ���ط���list
        /// </summary>
        /// <typeparam name="T">����1</typeparam>
        /// <param name="tag">�ڵ�</param>
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
                    throw new Exception("ϵͳ����!");
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
