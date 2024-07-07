using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.Schema;
using Common.Data.SqlClient;
using Dapper;
using SqlKata.Execution;

namespace ArticleAggregator.Core.Repositories.Implementations;

public class ArticleRepository : IArticleRepository
{
    private readonly QueryFactory _queryFactory;
    private readonly ISqlTransactionManager _sqlTransactionManager;

    private static readonly string InsertIntoSql;

    static ArticleRepository()
    {
        var columns = SqlDataMapper.GetColumnNames<Article>();

        var columnsString = string.Join(",", columns);
        var parametersString = string.Join(", ", columns.Select(c => "@" + c));

        InsertIntoSql = $"INSERT OR REPLACE INTO Article ({columnsString}) VALUES ({parametersString})";
    }

    public ArticleRepository(QueryFactory queryFactory, ISqlTransactionManager sqlTransactionManager)
    {
        _queryFactory = queryFactory;
        _sqlTransactionManager = sqlTransactionManager;
    }

    public async Task<Article?> Get(long articleId)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .FirstOrDefaultAsync<Article>();
    }

    public async Task<bool> Exists(long articleId)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .ExistsAsync();
    }

    public async Task<bool> Exists(Uri link)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Where(ArticleSchema.Columns.Link, link.ToString())
            .ExistsAsync();
    }

    public Task<int> Create(Article article)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Article article)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(long articleId)
    {
        throw new NotImplementedException();
    }

    public async Task UpsertMany(IList<Article> articles)
    {
        using var transaction = _sqlTransactionManager.BeginTransaction(_queryFactory.Connection);

        if (articles.Count == 0)
        {
            return;
        }

        _ = await _queryFactory.Connection.ExecuteAsync(InsertIntoSql, articles);

        transaction?.Commit();
    }
}