using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Schema;
using Common.Data.SqlClient;
using Common.Data.SqlKata.Utils.Filtering;
using Common.Data.SqlKata.Utils.Repositories;
using SqlKata.Execution;

namespace ArticleAggregator.Core.Repositories.Implementations;

public class ArticleRepository : Repository<Article>
{
    private readonly QueryFactory _queryFactory;
    private readonly ISqlTransactionManager _sqlTransactionManager;

    public ArticleRepository(
        QueryFactory queryFactory,
        ISqlTransactionManager sqlTransactionManager
    ) : base(ArticleSchema.TableName, queryFactory)
    {
        _queryFactory = queryFactory;
        _sqlTransactionManager = sqlTransactionManager;
    }

    public async Task<int> UpsertMany(IEnumerable<Article> articles)
    {
        using var transaction = _sqlTransactionManager.BeginTransaction(_queryFactory.Connection);

        var rowsAffected = 0;

        var enumerable = articles.ToArray();

        if (enumerable.Length == 0)
        {
            return rowsAffected;
        }

        var queryFilter = new QueryFilter();
        foreach (var article in enumerable)
        {
            queryFilter.FilterOperator = FilterOperator.Eq;
            queryFilter.Field = ArticleSchema.Columns.Link;
            queryFilter.Value = article.Link;

            if (await Exists(queryFilter))
            {
                rowsAffected += await Update(article, queryFilter, transaction);
            }
            else
            {
                rowsAffected += await Create(article, transaction);
            }
        }

        transaction?.Commit();

        return rowsAffected;
    }
}