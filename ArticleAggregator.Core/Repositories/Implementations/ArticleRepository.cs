using System.Data;
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
    private static readonly string InsertSql;

    static ArticleRepository()
    {
        var columns = SqlDataMapper.GetColumnNames<Article>();
        InsertSql = GenerateInsertStatement(columns);
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

    public async Task<Article?> Get(Uri link)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Where(ArticleSchema.Columns.Link, link)
            .FirstOrDefaultAsync<Article>();
    }

    public async Task<IEnumerable<Article>> GetMany(int page = 1, int pageSize = 100)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .GetAsync<Article>();
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
            .Where(ArticleSchema.Columns.Link, link)
            .ExistsAsync();
    }

    public async Task<int> Create(Article article, IDbTransaction? transaction = null)
    {
        return await _queryFactory.Connection.ExecuteAsync(InsertSql, article, transaction);
    }

    public async Task<int> Update(Article article, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Where(ArticleSchema.Columns.Link, article.Link)
            .UpdateAsync(new
            {
                article.Title,
                article.Summary,
                article.Author,
                article.Link,
                article.PublishDate,
                article.LastUpdatedTime,
                article.Source,
            }, transaction);
    }

    public async Task<int> Delete(long articleId)
    {
        return await _queryFactory
            .Query(ArticleSchema.TableName)
            .Where(ArticleSchema.Columns.Id, articleId)
            .DeleteAsync();
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

        foreach (var article in enumerable)
        {
            if (await Exists(article.Link))
            {
                rowsAffected += await Update(article, transaction);
            }
            else
            {
                rowsAffected += await Create(article, transaction);
            }
        }

        transaction?.Commit();

        return rowsAffected;
    }

    private static string GenerateInsertStatement(IList<string> columns)
    {
        var columnsString = string.Join(",", columns);
        var parametersString = string.Join(", ", columns.Select(c => "@" + c));

        return $"INSERT INTO {ArticleSchema.TableName} ({columnsString}) VALUES ({parametersString})";
    }
}