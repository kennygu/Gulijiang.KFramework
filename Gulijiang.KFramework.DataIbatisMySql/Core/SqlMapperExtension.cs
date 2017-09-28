/****************************************************************************
* 功能描述：    Ibatis的SqlMapper扩展类，实现分页功能
*****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.MappedStatements.PostSelectStrategy;
using IBatisNet.DataMapper.MappedStatements.ResultStrategy;
using IBatisNet.DataMapper.Scope;

namespace Gulijiang.KFramework.DataIbatisMySql.Core
{
    public static class SqlMapperExtension
    {
        private const string CountSql = "select count(*) {0}";

        public static IList QueryPageList(this ISqlMapper sqlMap, string statementName, Object parameter, string orderby, int offset, int limit)
        {
            bool flag = false;
            ISqlMapSession sqlMapSession = sqlMap.LocalSession;
            if (sqlMapSession == null)
            {
                sqlMapSession = sqlMap.CreateSqlMapSession();
                flag = true;
            }
            try
            {
                IMappedStatement statement = sqlMap.GetMappedStatement(statementName);
                RequestScope request = statement.Statement.Sql.GetRequestScope(statement, parameter, sqlMapSession);
                request.IDbCommand = new DbCommandDecorator(
                    sqlMapSession.CreateCommand(statement.Statement.CommandType), request);
                ApplyParameterMap(sqlMapSession, request.IDbCommand, request, statement.Statement, parameter);

                request.PreparedStatement.PreparedSql =
                    MySqlDialect.GetLimitString(request.PreparedStatement.PreparedSql, orderby, offset, limit);
                statement.PreparedCommand.Create(request, sqlMapSession, statement.Statement, parameter);
                return RunQueryForList(request, sqlMap.LocalSession, parameter, statement.Statement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (flag)
                {
                    sqlMapSession.CloseConnection();
                }
            }
        }

        public static IList<T> QueryForListWithPage<T>(this ISqlMapper sqlMap, string statementName, object parameter,
            string orderby, int offset, int limit, ref int totalCount)
        {
            bool flag = false;
            ISqlMapSession sqlMapSession = sqlMap.LocalSession;
            if (sqlMapSession == null)
            {
                sqlMapSession = sqlMap.CreateSqlMapSession();
                flag = true;
            }
            try
            {
                IMappedStatement statement = sqlMap.GetMappedStatement(statementName);
                RequestScope request = statement.Statement.Sql.GetRequestScope(statement, parameter, sqlMapSession);
                request.IDbCommand = new DbCommandDecorator(
                    sqlMapSession.CreateCommand(statement.Statement.CommandType), request);
                ApplyParameterMap(sqlMapSession, request.IDbCommand, request, statement.Statement, parameter);
                string cmdCountSql = string.Format(CountSql,
                    request.PreparedStatement.PreparedSql.Substring(
                        request.PreparedStatement.PreparedSql.ToLower().IndexOf("from", StringComparison.Ordinal)));

                totalCount = GetCount(request, sqlMapSession, cmdCountSql);

                request.PreparedStatement.PreparedSql =
                    MySqlDialect.GetLimitString(request.PreparedStatement.PreparedSql, orderby, offset, limit);
                statement.PreparedCommand.Create(request, sqlMapSession, statement.Statement, parameter);
                return (List<T>)RunQueryForList<T>(request, sqlMapSession, parameter, statement.Statement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (flag)
                {
                    sqlMapSession.CloseConnection();
                }
            }
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

        private static void ApplyParameterMap(ISqlMapSession session, IDbCommand command, RequestScope request, IStatement statement, object parameterObject)
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
                        throw new Exception("A procedure statement tag must alway have a parameterMap attribute, which is not the case for the procedure '" + statement.Id + "'.");
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
                    object memberValue = ObjectProbe.GetMemberValue(dbDataParameter, parameterDbTypeProperty, request.DataExchangeFactory.AccessorFactory);
                    ObjectProbe.SetMemberValue(dbDataParameter2, parameterDbTypeProperty, memberValue, request.DataExchangeFactory.ObjectFactory, request.DataExchangeFactory.AccessorFactory);
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

        private static IList RunQueryForList(RequestScope request, ISqlMapSession session, object parameterObject, IStatement _statement)
        {
            IList list = null;
            using (IDbCommand command = request.IDbCommand)
            {
                list = (_statement.ListClass == null) ? (new ArrayList()) : (_statement.CreateInstanceOfListClass());
                IDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        object obj = ResultStrategyFactory.Get(_statement).Process(request, ref reader, null);
                        if (obj != BaseStrategy.SKIP)
                        {
                            list.Add(obj);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, command, parameterObject);
            }
            return list;
        }

        private static IList<T> RunQueryForList<T>(RequestScope request, ISqlMapSession session, object parameterObject, IStatement _statement)
        {
            IList<T> list = new List<T>();
            using (IDbCommand command = request.IDbCommand)
            {
                list = (_statement.ListClass == null) ? (new List<T>()) : (_statement.CreateInstanceOfGenericListClass<T>());
                IDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        object obj = ResultStrategyFactory.Get(_statement).Process(request, ref reader, null);
                        if (obj != BaseStrategy.SKIP)
                        {
                            list.Add((T)obj);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, command, parameterObject);
            }
            return list;
        }

        private static void ExecutePostSelect(RequestScope request)
        {
            while (request.QueueSelect.Count > 0)
            {
                PostBindind postSelect = request.QueueSelect.Dequeue() as PostBindind;
                PostSelectStrategyFactory.Get(postSelect.Method).Execute(postSelect, request);
            }
        }

        private static void RetrieveOutputParameters(RequestScope request, ISqlMapSession session, IDbCommand command, object result)
        {
            if (request.ParameterMap != null)
            {
                int count = request.ParameterMap.PropertiesList.Count;
                for (int i = 0; i < count; i++)
                {
                    IBatisNet.DataMapper.Configuration.ParameterMapping.ParameterProperty mapping = request.ParameterMap.GetProperty(i);
                    if (mapping.Direction == ParameterDirection.Output ||
                        mapping.Direction == ParameterDirection.InputOutput)
                    {
                        string parameterName = string.Empty;
                        if (session.DataSource.DbProvider.UseParameterPrefixInParameter == false)
                        {
                            parameterName = mapping.ColumnName;
                        }
                        else
                        {
                            parameterName = session.DataSource.DbProvider.ParameterPrefix +
                                mapping.ColumnName;
                        }

                        if (mapping.TypeHandler == null) // Find the TypeHandler
                        {
                            lock (mapping)
                            {
                                if (mapping.TypeHandler == null)
                                {
                                    Type propertyType = ObjectProbe.GetMemberTypeForGetter(result, mapping.PropertyName);

                                    mapping.TypeHandler = request.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(propertyType);
                                }
                            }
                        }

                        // Fix IBATISNET-239
                        //"Normalize" System.DBNull parameters
                        IDataParameter dataParameter = (IDataParameter)command.Parameters[parameterName];
                        object dbValue = dataParameter.Value;

                        object value = null;

                        bool wasNull = (dbValue == DBNull.Value);
                        if (wasNull)
                        {
                            if (mapping.HasNullValue)
                            {
                                value = mapping.TypeHandler.ValueOf(mapping.GetAccessor.MemberType, mapping.NullValue);
                            }
                            else
                            {
                                value = mapping.TypeHandler.NullValue;
                            }
                        }
                        else
                        {
                            value = mapping.TypeHandler.GetDataBaseValue(dataParameter.Value, result.GetType());
                        }

                        request.IsRowDataFound = request.IsRowDataFound || (value != null);

                        request.ParameterMap.SetOutputParameter(ref result, mapping, value);
                    }
                }
            }
        }
    }


}