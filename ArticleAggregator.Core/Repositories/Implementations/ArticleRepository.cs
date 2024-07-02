using ArticleAggregator.Core.DataModels;
using ArticleAggregator.Core.Repositories.Interfaces;

namespace ArticleAggregator.Core.Repositories.Implementations;

public class ArticleRepository : IArticleRepository
{
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

    public Task CreateMany(IList<Article> articles)
    {
        throw new NotImplementedException();
    }
}