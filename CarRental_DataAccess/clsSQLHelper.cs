using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CarRental_DataAccess
{
    public static class clsSQLHelper
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["CarRentalDB"].ConnectionString;

        // ================= SESSION CONTEXT =================

        public static clsDbSessionContext CurrentContext { get; set; } =
            new clsDbSessionContext
            {
                UserID = 1,
                MachineName = Environment.MachineName,
                IPAddress = "127.0.0.1",
                Source = "UI"
            };

        // ================= CONNECTION =================

        private static async Task<SqlConnection> CreateOpenConnectionAsync()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync().ConfigureAwait(false);

            await ApplySessionContext(connection).ConfigureAwait(false);

            return connection;
        }

        // ================= APPLY SESSION =================

        private static async Task ApplySessionContext(SqlConnection connection)
        {
            if (CurrentContext == null)
                throw new Exception("DbSessionContext not initialized.");

            using (var cmd = new SqlCommand(@"
             EXEC sp_set_session_context 'UserID', @UserID;
             EXEC sp_set_session_context 'CorrelationID', @CorrelationID;
             EXEC sp_set_session_context 'Source', @Source;
             EXEC sp_set_session_context 'MachineName', @Machine;
             EXEC sp_set_session_context 'IPAddress', @IP;
             ", connection))
            {
                // توليد GUID واحد للعملية إذا لم يكن موجوداً
                var correlationId = Guid.NewGuid().ToString();

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = CurrentContext.UserID;
                cmd.Parameters.Add("@CorrelationID", SqlDbType.NVarChar, 50).Value = correlationId;
                cmd.Parameters.Add("@Source", SqlDbType.NVarChar, 100).Value = (object)CurrentContext.Source ?? "UI";
                cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 100).Value = (object)CurrentContext.MachineName ?? Environment.MachineName;
                cmd.Parameters.Add("@IP", SqlDbType.NVarChar, 100).Value = (object)CurrentContext.IPAddress ?? "127.0.0.1";

                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        // ================= EXECUTE NON QUERY =================

        public static async Task<int> ExecuteNonQueryAsync(string SP, Action<SqlParameterCollection> paramBuilder = null)
        {
            using (var connection = await CreateOpenConnectionAsync())
            using (var command = new SqlCommand(SP, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                paramBuilder?.Invoke(command.Parameters);

                return await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        // ================= EXECUTE SCALAR =================

        public static async Task<object> ExecuteScalarAsync(string SP, Action<SqlParameterCollection> paramBuilder = null)
        {
            using (var connection = await CreateOpenConnectionAsync())
            using (var command = new SqlCommand(SP, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                paramBuilder?.Invoke(command.Parameters);

                return await command.ExecuteScalarAsync().ConfigureAwait(false);
            }
        }

        // ================= EXECUTE READER =================

        public static async Task<List<T>> ExecuteReaderAsync<T>(string SP,
            Func<SqlDataReader, T> mapper,
            Action<SqlParameterCollection> paramBuilder = null)
        {
            var list = new List<T>();

            using (var connection = await CreateOpenConnectionAsync())
            using (var command = new SqlCommand(SP, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                paramBuilder?.Invoke(command.Parameters);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                        list.Add(mapper(reader));
                }
            }

            return list;
        }

        // ================= EXECUTE DATATABLE =================

        public static async Task<DataTable> ExecuteDataTableAsync(string SP, Action<SqlParameterCollection> paramBuilder = null)
        {
            var dt = new DataTable();

            using (var connection = await CreateOpenConnectionAsync())
            using (var command = new SqlCommand(SP, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                paramBuilder?.Invoke(command.Parameters);

                using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    dt.Load(reader);
            }

            return dt;
        }

        // ================= HELPERS =================

        public static Dictionary<string, int> GetOrdinal(SqlDataReader reader, params string[] columns)
        {
            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (string col in columns)
                dict[col] = reader.GetOrdinal(col);

            return dict;
        }

        public static string GetStringOrNull(this SqlDataReader reader, int index)
            => reader.IsDBNull(index) ? null : reader.GetString(index);

        public static int? GetIntOrNull(this SqlDataReader reader, int index)
            => reader.IsDBNull(index) ? (int?)null : reader.GetInt32(index);

        public static DateTime? GetDateTimeOrNull(this SqlDataReader reader, int index)
            => reader.IsDBNull(index) ? (DateTime?)null : reader.GetDateTime(index);
        public static decimal? GetDecimalOrNull(this SqlDataReader reader, int index)
            => reader.IsDBNull(index) ? (decimal?)null : reader.GetDecimal(index);
        public static int? ToInt32Safe(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToInt32(value);
        }
        public static decimal? ToDecimalSafe(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            return Convert.ToDecimal(value);
        }

        public static SqlParameter CreateDecimalParameter(string parameterName, decimal value, int precision = 10, int scale = 2)
        {
            return new SqlParameter(parameterName, SqlDbType.Decimal)
            {
                Precision = Convert.ToByte(precision),
                Scale = Convert.ToByte(scale),
                Value = value
            };
        }
    }
}