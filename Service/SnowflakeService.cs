using Snowflake.Data.Client;
using System.Data;

namespace Datahub_System_Goal.Service
{
    public class SnowflakeService
    {
        private readonly SnowflakeDbConnection _connection;
        public SnowflakeService(SnowflakeDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<DataTable> GetDataFromTableAsync(string tableName)
        {
            await _connection.OpenAsync();
            using (SnowflakeDbCommand cmd = (SnowflakeDbCommand)_connection.CreateCommand())
            {
                cmd.CommandText = $"Select * from {tableName}";
                using (var adapter = new SnowflakeDbDataAdapter(cmd))
                {
                    var datatTable = new DataTable();
                    adapter.Fill(datatTable);
                    return datatTable;
                }
            }
        }
    }
}
