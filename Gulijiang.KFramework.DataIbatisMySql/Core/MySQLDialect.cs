using System.Text;

namespace Gulijiang.KFramework.DataIbatisMySql.Core
{
    public static class MySqlDialect
    {
        private const string SqlEndDelimiter = ";";
        private const string SqlOrderBy = " order by {0} ";
        public static string GetLimitString(string sql, string orderby, int offset, int limit)
        {
            sql = trim(sql);
            StringBuilder sb = new StringBuilder(sql.Length + 20);
            sb.Append(sql);
            if (orderby.Length > 0)
            {
                sb.Append(string.Format(SqlOrderBy, orderby));
            }
            if (offset > 0)
            {
                sb.Append(" limit ").Append(offset).Append(',').Append(limit)
                        .Append(SqlEndDelimiter);
            }
            else
            {
                sb.Append(" limit ").Append(limit).Append(SqlEndDelimiter);
            }
            return sb.ToString();
        }

        public static bool SupportsLimit()
        {
            return true;
        }

        private static string trim(string sql)
        {
            sql = sql.Trim();
            if (sql.EndsWith(SqlEndDelimiter))
            {
                sql = sql.Substring(0, sql.Length - 1
                        - SqlEndDelimiter.Length);
            }
            return sql;
        }


    }
}
