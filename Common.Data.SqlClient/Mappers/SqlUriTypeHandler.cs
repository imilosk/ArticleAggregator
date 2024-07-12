using System.Data;
using Dapper;

namespace Common.Data.SqlClient.Mappers;

public class SqlUriTypeHandler : SqlMapper.TypeHandler<Uri>
{
    public override void SetValue(IDbDataParameter parameter, Uri value)
    {
        parameter.Value = value.ToString();
    }

    public override Uri Parse(object value)
    {
        return new Uri((string)value);
    }
}