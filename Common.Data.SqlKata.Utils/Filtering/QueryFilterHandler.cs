using SqlKata;

namespace Common.Data.SqlKata.Utils.Filtering;

public static class QueryFilterHandler
{
    internal static Query ApplyFilters(Query query, IEnumerable<QueryFilter> queryFilters)
    {
        foreach (var filter in queryFilters)
        {
            ApplyFilter(query, filter);
        }

        return query;
    }

    internal static Query ApplyFilter(Query query, QueryFilter queryFilter)
    {
        switch (queryFilter.FilterOperator)
        {
            case FilterOperator.Eq
                or FilterOperator.Neq
                or FilterOperator.Gt
                or FilterOperator.Lt
                or FilterOperator.Gte
                or FilterOperator.Lte:
                query.Where(queryFilter.Field, GetSqlOperator(queryFilter.FilterOperator),
                    queryFilter.Value);
                break;
            case FilterOperator.Like:
                query.WhereLike(queryFilter.Field, queryFilter.Value);
                break;
            case FilterOperator.IsNull:
                query.WhereNull(queryFilter.Field);
                break;
            case FilterOperator.IsNotNull:
                query.WhereNotNull(queryFilter.Field);
                break;
            case FilterOperator.Offset:
                query.Offset((int)(queryFilter.Value ?? 0));
                break;
            case FilterOperator.Limit:
                query.Limit((int)(queryFilter.Value ?? 0));
                break;
            default:
                throw new ArgumentException($"Unsupported filter operator {queryFilter.FilterOperator}");
        }

        return query;
    }

    private static string GetSqlOperator(FilterOperator filterOperator)
    {
        return filterOperator switch
        {
            FilterOperator.Eq => "=",
            FilterOperator.Neq => "<>",
            FilterOperator.Gt => ">",
            FilterOperator.Gte => ">=",
            FilterOperator.Lt => "<",
            FilterOperator.Lte => "<=",
            _ => throw new NotSupportedException($"The filter operator '{filterOperator}' is not supported.")
        };
    }
}