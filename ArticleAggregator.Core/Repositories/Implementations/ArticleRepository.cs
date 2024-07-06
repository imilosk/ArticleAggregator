using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Repositories.Interfaces;
using ArticleAggregator.Schema;
using SqlKata.Execution;

namespace ArticleAggregator.Core.Repositories.Implementations;

public class ArticleRepository : IArticleRepository
{
    private readonly QueryFactory _queryFactory;

    private static readonly string[] Columns =
    [
        ArticleSchema.Columns.Title,
        ArticleSchema.Columns.Summary,
        ArticleSchema.Columns.Author,
        ArticleSchema.Columns.Link,
        ArticleSchema.Columns.PublishDate,
        ArticleSchema.Columns.LastUpdatedTime,
    ];

    public ArticleRepository(QueryFactory queryFactory)
    {
        _queryFactory = queryFactory;
    }

    public Task<Article> Get(long articleId)
    {
        throw new NotImplementedException();
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
        var data = new List<object[]>();
        foreach (var article in articles)
        {
            data.Add(new object[]
            {
                article.Title,
                article.Summary,
                article.Author,
                article.Link.ToString(),
                article.PublishDate,
                article.LastUpdatedTime,
            });
        }

        var query = _queryFactory
            .Query(ArticleSchema.TableName)
            .AsInsert(Columns, data);

        await _queryFactory.ExecuteAsync(query);
    }
}