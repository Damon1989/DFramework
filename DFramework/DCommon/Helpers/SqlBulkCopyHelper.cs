namespace DCommon
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Linq;

    public class SqlBulkCopyHelper
    {
        private static readonly string Conn = ConfigurationManager.AppSettings["dbConn"];

        public static DataTable GetDataTable<T>(List<T> data)
        {
            var dt = new DataTable();
            var properties = data.First().GetType().GetProperties();
            properties.ToList().ForEach(
                propertyinfo =>
                    {
                        if (propertyinfo.PropertyType.ToString().Equals(typeof(Guid).ToString()))
                            dt.Columns.Add(propertyinfo.Name, typeof(SqlGuid));
                        else dt.Columns.Add(propertyinfo.Name);
                    });

            data.ForEach(
                item =>
                    {
                        var dr = dt.NewRow();
                        var proties = item.GetType().GetProperties();
                        foreach (var info in proties)
                        {
                            var value = info.GetValue(item);
                            if (info.PropertyType.ToString().Equals(typeof(Guid))) dr[info.Name] = (SqlGuid)value;
                            else dr[info.Name] = value;
                        }

                        dt.Rows.Add(dr);
                    });

            return dt;
        }

        public static bool InsertDb<T>(
            List<T> data,
            string dataTableName,
            string connectionString,
            params string[] excludeFields)
        {
            try
            {
                if (!data.Any()) return false;

                using (var conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    using (var sqlBulkCopy = new SqlBulkCopy(conn))
                    {
                        var dt = GetDataTable(data);

                        var properties = data.First().GetType().GetProperties();
                        properties.ToList().ForEach(
                            propertyinfo =>
                                {
                                    if (!propertyinfo.GetCustomAttributes(false).Any(p => p is NotMappedAttribute))
                                        if (!excludeFields.Contains(propertyinfo.Name))
                                            sqlBulkCopy.ColumnMappings.Add(propertyinfo.Name, propertyinfo.Name);
                                });

                        sqlBulkCopy.DestinationTableName = dataTableName;
                        sqlBulkCopy.WriteToServer(dt);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public static bool SaveDb<T>(List<T> data, string dataTableName, string connectionString = "")
        {
            if (!data.Any()) return false;

            connectionString = connectionString == string.Empty ? Conn : connectionString;
            var sqlBulkCopy = new SqlBulkCopy(connectionString);
            try
            {
                var dt = new DataTable();
                var properties = data.First().GetType().GetProperties();
                properties.ToList().ForEach(
                    propertyinfo =>
                        {
                            dt.Columns.Add(propertyinfo.Name);
                            sqlBulkCopy.ColumnMappings.Add(propertyinfo.Name, propertyinfo.Name);
                        });

                data.ForEach(
                    item =>
                        {
                            var dr = dt.NewRow();
                            var proties = item.GetType().GetProperties();

                            foreach (var info in proties)
                            {
                                var value = info.GetValue(item);
                                dr[info.Name] = value;
                            }

                            dt.Rows.Add(dr);
                        });

                sqlBulkCopy.DestinationTableName = dataTableName;

                sqlBulkCopy.WriteToServer(dt);
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                sqlBulkCopy.Close();
            }

            return true;
        }

        public static bool UpdateDb<T>(
            List<T> data,
            string dataTableName,
            string connectionString,
            string updateItemSql,
            string primaryKey)
        {
            try
            {
                if (!data.Any()) return false;

                var tempDataTableName = $"#{dataTableName}";

                using (var conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var createTableSql = $"select * into #{dataTableName} from {dataTableName} where 1=2";

                    var sqlCommand = new SqlCommand
                                         {
                                             CommandText = createTableSql,
                                             CommandType = CommandType.Text,
                                             Connection = conn
                                         };
                    sqlCommand.ExecuteNonQuery();

                    using (var sqlBulkCopy = new SqlBulkCopy(conn))
                    {
                        var dt = GetDataTable(data);

                        var properties = data.First().GetType().GetProperties();
                        properties.ToList().ForEach(
                            propertyinfo =>
                                {
                                    if (!propertyinfo.GetCustomAttributes(false).Any(p => p is NotMappedAttribute))
                                        sqlBulkCopy.ColumnMappings.Add(propertyinfo.Name, propertyinfo.Name);
                                });

                        sqlBulkCopy.DestinationTableName = tempDataTableName;
                        sqlBulkCopy.WriteToServer(dt);
                    }

                    var updateSql = string.Format(
                        "update t2 SET {2}  FROM  {0} AS t1,{1} AS t2 WHERE t1.{3} = t2.{3}",
                        tempDataTableName,
                        dataTableName,
                        updateItemSql,
                        primaryKey);

                    sqlCommand = new SqlCommand
                                     {
                                         CommandText = updateSql, CommandType = CommandType.Text, Connection = conn
                                     };
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }
    }
}