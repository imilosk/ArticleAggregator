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

    private static readonly string UpsertSql;
    private static readonly string InsertSql;

    static ArticleRepository()
    {
        var columns = SqlDataMapper.GetColumnNames<Article>();

        var columnsString = string.Join(",", columns);
        var parametersString = string.Join(", ", columns.Select(c => "@" + c));

        InsertSql = $"INSERT OR IGNORE INTO Article ({columnsString}) VALUES ({parametersString})";
        UpsertSql = $"INSERT OR REPLACE INTO Article ({columnsString}) VALUES ({parametersString})";
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

    public async Task<int> Create(Article article)
    {
        return await _queryFactory.Connection.ExecuteAsync(InsertSql, article);
    }

    public async Task<int> Update(Article article)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Where(ArticleSchema.Columns.Id, article.Id)
            .UpdateAsync(article);
    }

    public async Task<int> Delete(long articleId)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Where(ArticleSchema.Columns.Id, articleId)
            .DeleteAsync();
    }

    public async Task<int> UpsertMany(IList<Article> articles)
    {
        using var transaction = _sqlTransactionManager.BeginTransaction(_queryFactory.Connection);

        if (articles.Count == 0)
        {
            return 0;
        }

        var rowsAffected = await _queryFactory.Connection.ExecuteAsync(UpsertSql, articles, transaction);

        transaction?.Commit();

        return rowsAffected;
    }
}