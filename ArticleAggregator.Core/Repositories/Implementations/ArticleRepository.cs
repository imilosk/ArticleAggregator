using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.Schema;
using IMilosk.Data.SqlClient.DatabaseConnector.Interfaces;
using IMilosk.Data.SqlKata.Utils.Filtering;
using IMilosk.Data.SqlKata.Utils.Repositories;
using SqlKata.Execution;

namespace ArticleAggregator.Core.Repositories.Implementations;

public class ArticleRepository : Repository<Article>, IArticleRepository
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

    public async Task<(int Inserted, int Updated)> UpsertMany(IEnumerable<Article> articles)
    {
        using var transaction = _sqlTransactionManager.BeginTransaction(_queryFactory.Connection);

        var inserted = 0;
        var updated = 0;

        var enumerable = articles.ToArray();

        if (enumerable.Length == 0)
        {
            return (inserted, updated);
        }

        var queryFilter = new QueryFilter();
        foreach (var article in enumerable)
        {
            queryFilter.FilterOperator = FilterOperator.Eq;
            queryFilter.Field = ArticleSchema.Columns.Link;
            queryFilter.Value = article.Link;

            if (await Exists(queryFilter))
            {
                updated += await Update(article, queryFilter, transaction);
            }
            else
            {
                inserted += await Create(article, transaction);
            }
        }

        transaction?.Commit();

        return (inserted, updated);
    }
}