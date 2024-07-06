using System.Data;

namespace Common.Data.SqlClient;

public interface ISqlTransactionManager
{
    IDbTransaction? BeginTransaction(IDbConnection connection);
}