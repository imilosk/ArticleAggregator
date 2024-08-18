namespace Common.Data.SqlKata.Utils.Filtering;

public enum FilterOperator
{
    None,
    Eq,
    Neq,
    Gt,
    Lt,
    Gte,
    Lte,
    Like,
    IsNull,
    IsNotNull,
    Limit,
    Offset
}