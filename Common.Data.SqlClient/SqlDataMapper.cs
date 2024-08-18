using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Common.Data.SqlClient;

public static class SqlDataMapper
{
    public static IList<DtoColumnMapping> GetColumnMapping<T>()
    {
        var type = typeof(T);
        var columns = new List<DtoColumnMapping>();

        foreach (var fieldInfo in type.GetProperties())
        {
            if (ShouldSkip(fieldInfo))
            {
                continue;
            }

            columns.Add(new DtoColumnMapping
            {
                FieldName = fieldInfo.Name,
                TableColumnName = GetDatabaseFieldName(fieldInfo),
            });
        }

        return columns;
    }

    public static IList<string> GetColumnNames<T>()
    {
        var columnMapping = GetColumnMapping<T>();

        return columnMapping.Select(mapping => mapping.FieldName).ToList();
    }

    public static Dictionary<string, object?> GetAnonymousObject<T>(T entity)
    {
        var type = typeof(T);
        var dict = new Dictionary<string, object?>();

        foreach (var fieldInfo in type.GetProperties())
        {
            if (ShouldSkip(fieldInfo))
            {
                continue;
            }

            dict.Add(GetDatabaseFieldName(fieldInfo), fieldInfo.GetValue(entity));
        }

        return dict;
    }

    private static bool ShouldSkip(PropertyInfo fieldInfo)
    {
        var notMappedAttribute = fieldInfo.GetCustomAttribute(typeof(NotMappedAttribute), false);

        return notMappedAttribute is not null;
    }

    private static string GetDatabaseFieldName(PropertyInfo fieldInfo)
    {
        var columnAttribute = (ColumnAttribute?)fieldInfo.GetCustomAttribute(typeof(ColumnAttribute), false);

        return columnAttribute?.Name ?? fieldInfo.Name;
    }
}

public class DtoColumnMapping
{
    public string FieldName = string.Empty;
    public string TableColumnName = string.Empty;
}