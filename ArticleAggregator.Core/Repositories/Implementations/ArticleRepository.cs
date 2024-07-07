using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.Schema;
using Common.Data.SqlClient;
using SqlKata.Execution;

namespace ArticleAggregator.Core.Repositories.Implementations;

public class ArticleRepository : IArticleRepository
{
    private readonly QueryFactory _queryFactory;
    private readonly ISqlTransactionManager _sqlTransactionManager;

    private static readonly IList<string> Columns;

    static ArticleRepository()
    {
        Columns = SqlDataMapper.GetColumnNames<Article>();
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

    public async Task CreateMany(IList<Article> articles)
    {
        using var transaction = _sqlTransactionManager.BeginTransaction(_queryFactory.Connection);

        var data = new List<object[]>();
        foreach (var article in articles)
        {
            data.Add([
                article.Title,
                article.Summary,
                article.Author,
                article.Link.ToString(),
                article.PublishDate,
                article.LastUpdatedTime,
            ]);
        }

        if (data.Count == 0)
        {
            return;
        }

        var query = _queryFactory
            .Query(ArticleSchema.TableName)
            .AsInsert(Columns, data);

        await _queryFactory.ExecuteAsync(query, transaction);

        transaction?.Commit();
    }

    public async Task CreateManyIfNotExists(IList<Article> articles)
    {
        using var transaction = _sqlTransactionManager.BeginTransaction(_queryFactory.Connection);

        var data = new List<object[]>();
        foreach (var article in articles)
        {
            var articleExists = await Exists(article.Link);
            if (!articleExists)
            {
                data.Add([
                    article.Title,
                    article.Summary,
                    article.Author,
                    article.Link.ToString(),
                    article.PublishDate,
                    article.LastUpdatedTime,
                ]);
            }
        }

        if (data.Count == 0)
        {
            return;
        }

        var query = _queryFactory
            .Query(ArticleSchema.TableName)
            .AsInsert(Columns, data);

        await _queryFactory.ExecuteAsync(query, transaction);

        transaction?.Commit();
    }
}