/****************************************************************************
* 功能描述：    Ibatis的SqlMapper扩展类，实现分页功能by kenny add 2016-01-01
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.MappedStatements.PostSelectStrategy;
using IBatisNet.DataMapper.MappedStatements.ResultStrategy;
using IBatisNet.DataMapper.Scope;

namespace Gulijiang.KFramework.DataIbatis.Core
{
    public static class SqlMapperExtension
    {
        private const string PageSql = "with cte as( select id0=row_number() over(order by {0}),* from  ({1}) as cte1) select * from cte where id0 between @beginNo and @endNo";

        private const string CountSql = "select count(*) {0}";

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="mapper">mapper</param>
        /// <param name="tag">SQL Statement的id</param>
        /// <param name="paramObject">参数</param>
        /// <param name="orderby">查询条件，必须确保数据库中有这一列</param>
        /// <param name="beginNo">开始行数</param>
        /// <param name="endNo">结束行数</param>
        /// <param name="totalCount">总条数</param>
        /// <returns>查询结果</returns>
        public static IList<T> QueryForListWithPage<T>(this ISqlMapper mapper, string tag, object paramObject,string orderby, int beginNo, int endNo, ref int totalCount)
        {
            bool flag = false;
            ISqlMapSession sqlMapSession = mapper.LocalSession;
            if (sqlMapSession == null)
            {
                sqlMapSession = mapper.CreateSqlMapSession();
                flag = true;
            }
            try
            {
                IMappedStatement mappedStatement = mapper.GetMappedStatement(tag);
                IStatement statement = mappedStatement.Statement;
                RequestScope request = statement.Sql.GetRequestScope(mappedStatement, paramObject, sqlMapSession);
                string statementsql = request.PreparedStatement.PreparedSql;
                string cmdPageSql = string.Format(PageSql, orderby, statementsql);
                string cmdCountSql = string.Format(CountSql, statementsql.Substring(statementsql.ToLower().IndexOf("from", StringComparison.Ordinal)));

              //  request.PreparedStatement.PreparedSql = cmdPageSql;
                request.IDbCommand = new DbCommandDecorator(sqlMapSession.CreateCommand(statement.CommandType), request);
                ApplyParameterMap(sqlMapSession, request.IDbCommand, request, statement, paramObject);
                totalCount = GetCount(request, sqlMapSession, cmdCountSql);

               // request.IDbCommand.CommandText = request.PreparedStatement.PreparedSql;
                request.IDbCommand.CommandText = cmdPageSql;
                AddCommandParameters(beginNo, endNo, request);
                IList<T> result = RunQueryForList<T>(statement, request, sqlMapSession, paramObject, null, null);
                return result;
            }
            finally
            {
                if (flag)
                {
                    sqlMapSession.CloseConnection();
                }
            }
        }

        private static void AddCommandParameters(int beginNo, int endNo, RequestScope request)
        {
            IDataParameter begin = new SqlParameter();
            begin.ParameterName = "beginno";
            begin.DbType = DbType.Int32;
            begin.Value = beginNo;
            IDataParameter end = new SqlParameter();
            end.ParameterName = "endno";
            end.DbType = DbType.Int32;
            end.Value = endNo;
            request.IDbCommand.Parameters.Add(begin);
            request.IDbCommand.Parameters.Add(end);
        }

        private static int GetCount(RequestScope request, ISqlMapSession sqlMapSession, string cmdCountSql)
        {
            int totalCount;
            IDbCommand cmdCount = sqlMapSession.CreateCommand(CommandType.Text);
            cmdCount.Connection = sqlMapSession.Connection;
            cmdCount.CommandText = cmdCountSql;
            foreach (IDbDataParameter para in request.IDbCommand.Parameters)
            {
                IDbDataParameter cmdp = cmdCount.CreateParameter();
                cmdp.Direction = para.Direction;
                cmdp.Size = para.Size;
                cmdp.Precision = para.Precision;
                cmdp.Scale = para.Scale;
                cmdp.ParameterName = para.ParameterName;
                cmdp.Value = para.Value;
                cmdCount.Parameters.Add(cmdp);
            }
            cmdCount.Connection.Open();
            totalCount = Convert.ToInt32(cmdCount.ExecuteScalar());
            cmdCount.Connection.Close();
            return totalCount;
        }


        private static void ApplyParameterMap(ISqlMapSession session, IDbCommand command, RequestScope request,IStatement statement, object parameterObject)
        {
            StringCollection dbParametersName = request.PreparedStatement.DbParametersName;
            IDbDataParameter[] dbParameters = request.PreparedStatement.DbParameters;
            int count = dbParametersName.Count;
            for (int i = 0; i < count; i++)
            {
                IDbDataParameter dbDataParameter = dbParameters[i];
                IDbDataParameter dbDataParameter2 = command.CreateParameter();
                ParameterProperty property = request.ParameterMap.GetProperty(i);

                if (command.CommandType == CommandType.StoredProcedure)
                {
                    if (request.ParameterMap == null)
                    {
                        throw new Exception("A procedure statement tag must alway have a parameterMap attribute, which is not the case for the procedure '" +statement.Id + "'.");
                    }
                    if (property.DirectionAttribute.Length == 0)
                    {
                        property.Direction = dbDataParameter.Direction;
                    }
                    dbDataParameter.Direction = property.Direction;
                }

                request.ParameterMap.SetParameter(property, dbDataParameter2, parameterObject);
                dbDataParameter2.Direction = dbDataParameter.Direction;
                if (request.ParameterMap != null && property.DbType != null && property.DbType.Length > 0)
                {
                    string parameterDbTypeProperty = session.DataSource.DbProvider.ParameterDbTypeProperty;
                    object memberValue = ObjectProbe.GetMemberValue(dbDataParameter, parameterDbTypeProperty,request.DataExchangeFactory.AccessorFactory);
                    ObjectProbe.SetMemberValue(dbDataParameter2, parameterDbTypeProperty, memberValue,request.DataExchangeFactory.ObjectFactory,request.DataExchangeFactory.AccessorFactory);
                }

                if (session.DataSource.DbProvider.SetDbParameterSize && dbDataParameter.Size > 0)
                {
                    dbDataParameter2.Size = dbDataParameter.Size;
                }
                if (session.DataSource.DbProvider.SetDbParameterPrecision)
                {
                    dbDataParameter2.Precision = dbDataParameter.Precision;
                }
                if (session.DataSource.DbProvider.SetDbParameterScale)
                {
                    dbDataParameter2.Scale = dbDataParameter.Scale;
                }
                dbDataParameter2.ParameterName = dbDataParameter.ParameterName;
                command.Parameters.Add(dbDataParameter2);
            }
        }

        private static IList<T> RunQueryForList<T>(IStatement statement, RequestScope request, ISqlMapSession session, object parameterObject, IList<T> resultObject, RowDelegate<T> rowDelegate)
        {
            IList<T> list = resultObject;
            using (IDbCommand iDbCommand = request.IDbCommand)
            {
                if (resultObject == null)
                {
                    if (statement.ListClass == null)
                    {
                        list = new List<T>();
                    }
                    else
                    {
                        list = statement.CreateInstanceOfGenericListClass<T>();
                    }
                }
                IDataReader dataReader = iDbCommand.ExecuteReader();
                try
                {
                    IResultStrategy resultStrategy = ResultStrategyFactory.Get(statement);
                    do
                    {
                        if (rowDelegate == null)
                        {
                            while (dataReader.Read())
                            {
                                object obj = resultStrategy.Process(request, ref dataReader, null);
                                if (obj != BaseStrategy.SKIP)
                                {
                                    list.Add((T)(obj));
                                }
                            }
                        }
                        else
                        {
                            while (dataReader.Read())
                            {
                                var t = (T)(resultStrategy.Process(request, ref dataReader, null));
                                rowDelegate(t, parameterObject, list);
                            }
                        }
                    } while (dataReader.NextResult());
                }
                finally
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, iDbCommand, parameterObject);
            }
            return list;
        }

        internal static IList<T> RunQueryForList<T>(IStatement statement, RequestScope request, ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList<T> list;
            using (IDbCommand iDbCommand = request.IDbCommand)
            {
                if (statement.ListClass == null)
                {
                    list = new List<T>();
                }
                else
                {
                    list = statement.CreateInstanceOfGenericListClass<T>();
                }
                IDataReader dataReader = iDbCommand.ExecuteReader();
                try
                {
                    IResultStrategy resultStrategy = ResultStrategyFactory.Get(statement);
                    int num = 0;
                    while (num < skipResults && dataReader.Read())
                    {
                        num++;
                    }
                    int num2 = 0;
                    while ((maxResults == -1 || num2 < maxResults) && dataReader.Read())
                    {
                        object obj = resultStrategy.Process(request, ref dataReader, null);
                        if (obj != BaseStrategy.SKIP)
                        {
                            list.Add((T)(obj));
                        }
                        num2++;
                    }
                }
                finally
                {
                    dataReader.Close();
                    dataReader.Dispose();
                }
                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, iDbCommand, parameterObject);
            }
            return list;
        }

        private static void ExecutePostSelect(RequestScope request)
        {
            while (request.QueueSelect.Count > 0)
            {
                var postBindind = request.QueueSelect.Dequeue() as PostBindind;
                if (postBindind != null)
                    PostSelectStrategyFactory.Get(postBindind.Method).Execute(postBindind, request);
            }
        }

        private static void RetrieveOutputParameters(RequestScope request, ISqlMapSession session, IDbCommand command, object result)
        {
            if (request.ParameterMap == null)
                return;

            int count = request.ParameterMap.PropertiesList.Count;
            for (int i = 0; i < count; i++)
            {
                ParameterProperty property = request.ParameterMap.GetProperty(i);
                if (property.Direction == ParameterDirection.Output || property.Direction == ParameterDirection.InputOutput)
                {
                    string parameterName;
                    if (!session.DataSource.DbProvider.UseParameterPrefixInParameter)
                    {
                        parameterName = property.ColumnName;
                    }
                    else
                    {
                        parameterName = session.DataSource.DbProvider.ParameterPrefix + property.ColumnName;
                    }
                    if (property.TypeHandler == null)
                    {
                        ParameterProperty obj;
                        Monitor.Enter(obj = property);
                        try
                        {
                            if (property.TypeHandler == null)
                            {
                                Type memberTypeForGetter = ObjectProbe.GetMemberTypeForGetter(result,property.PropertyName);
                                property.TypeHandler = request.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(memberTypeForGetter);
                            }
                        }
                        finally
                        {
                            Monitor.Exit(obj);
                        }
                    }
                    var dataParameter = (IDataParameter)command.Parameters[parameterName];
                    object value = dataParameter.Value;
                    bool flag = value == DBNull.Value;
                    object obj2;
                    if (flag)
                    {
                        if (property.HasNullValue)
                        {
                            obj2 = property.TypeHandler.ValueOf(property.GetAccessor.MemberType, property.NullValue);
                        }
                        else
                        {
                            obj2 = property.TypeHandler.NullValue;
                        }
                    }
                    else
                    {
                        obj2 = property.TypeHandler.GetDataBaseValue(dataParameter.Value, result.GetType());
                    }
                    request.IsRowDataFound = (request.IsRowDataFound || obj2 != null);
                    request.ParameterMap.SetOutputParameter(ref result, property, obj2);
                }
            }
        }
    }
}