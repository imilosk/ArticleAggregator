using System.Data;

namespace Common.Data.SqlClient;

public interface IDatabaseConnector
{
    IDbConnection GetConnection();
}