namespace Common.Data.SqlKata.Utils.Filtering;

public class ComparisonFilterBuilder
{
    private readonly List<QueryFilter> _filters = [];

    public ComparisonFilterBuilder AddFilter(string field, FilterOperator filterOperator, object value)
    {
        _filters.Add(new QueryFilter
        {
            Field = field,
            FilterOperator = filterOperator,
            Value = value
        });

        return this;
    }

    public ComparisonFilterBuilder AddFilter(string field, FilterOperator filterOperator)
    {
        _filters.Add(new QueryFilter
        {
            Field = field,
            FilterOperator = filterOperator
        });

        return this;
    }

    public ComparisonFilterBuilder AddFilter(FilterOperator filterOperator, object value)
    {
        _filters.Add(new QueryFilter
        {
            FilterOperator = filterOperator,
            Value = value
        });

        return this;
    }

    public IEnumerable<QueryFilter> Build()
    {
        return _filters;
    }
}