using System.Data;
using Microsoft.Data.Sqlite;

namespace Common.Data.SqlClient;

public class DatabaseConnector : IDatabaseConnector
{
    private readonly string _connectionString;

    public DatabaseConnector(DatabaseSettings settings)
    {
        _connectionString = settings.ConnectionStringTemplate
            .Replace("<FILE>", settings.File);
    }

    public IDbConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}