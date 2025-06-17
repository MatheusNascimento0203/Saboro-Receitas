using System.Data;
using System.Data.Common;
using Npgsql;
using NpgsqlTypes;
using DbException = Saboro.Core.Helpers.Exceptions.DbException;

namespace SmnRun.Data.Extensions
{
    public static class DbCommandExtension
    {
        private const string RETURN_VALUE = "@RETURN_VALUE";

        public static void OpenConnection(this DbCommand dbCommand)
        {
            try
            {
                if (dbCommand.Connection.State == ConnectionState.Broken)
                {
                    dbCommand.Connection.Close();
                    dbCommand.Connection.Open();
                }

                if (dbCommand.Connection.State == ConnectionState.Closed)
                    dbCommand.Connection.Open();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "53")
                    throw new DbException("Failed to establish connection to the database");

                throw new DbException(ex.ToString());
            }
        }

        public static async Task OpenConnectionAsync(this DbCommand dbCommand)
        {
            try
            {
                if (dbCommand.Connection.State == ConnectionState.Broken)
                {
                    await dbCommand.Connection.CloseAsync();
                    await dbCommand.Connection.OpenAsync();
                }

                if (dbCommand.Connection.State == ConnectionState.Closed)
                    await dbCommand.Connection.OpenAsync();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "53")
                    throw new DbException("Failed to establish connection to the database");

                throw new DbException(ex.ToString());
            }
        }

        public static DbCommand BuildCommand(this DbCommand dbCommand, CommandType commandType, string commandText,
            object parameters = null, int? timeout = null)
        {
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = commandText;

            if (timeout != null)
                dbCommand.CommandTimeout = (int)timeout;

            if (parameters != null)
                foreach (var property in parameters.GetType().GetProperties())
                {
                    var parameter = dbCommand.CreateParameter();
                    parameter.ParameterName = "p_" + property.Name;
                    parameter.Value = property.GetValue(parameters);
                    dbCommand.Parameters.Add(parameter);
                }

            return dbCommand;
        }

        public static void AddParameterOutput(this DbCommand dbCommand, string name, object value, NpgsqlDbType parameterType)
        {
            dbCommand.Parameters.Add(new NpgsqlParameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = name,
                Value = value,
                NpgsqlDbType = parameterType
            });
        }

        public static T GetParameterOutput<T>(this DbCommand dbCommand, string name)
        {
            var returnValue = dbCommand.Parameters[name].Value;
            if (returnValue == DBNull.Value)
                return default(T);

            return (T)Convert.ChangeType(returnValue, Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T));
        }

        public static void AddParameterReturnValue(this DbCommand dbCommand)
        {
            dbCommand.Parameters.Add(new NpgsqlParameter
            {
                Direction = ParameterDirection.ReturnValue,
                ParameterName = RETURN_VALUE,
                NpgsqlDbType = NpgsqlDbType.Integer
            });
        }

        public static T GetParameterReturnValue<T>(this DbCommand dbCommand)
        {
            return dbCommand.GetParameterOutput<T>(RETURN_VALUE);
        }

        public static T ExecuteNonQueryWithReturnValue<T>(this DbCommand dbCommand)
        {
            try
            {
                dbCommand.AddParameterReturnValue();
                dbCommand.ExecuteNonQuery();
                return dbCommand.GetParameterOutput<T>(RETURN_VALUE);
            }
            catch (PostgresException ex)
            {
                throw HandlePostgresException(ex);
            }
        }

        private static Exception HandlePostgresException(PostgresException ex)
        {
            return ex.SqlState switch
            {
                "53" => new DbException("Failed to establish connection to the database"),
                "1205" => new DbException("Service unavailable, please retry the operation in a few minutes or contact the DDP."),
                _ => ex,
            };
        }
    }
}