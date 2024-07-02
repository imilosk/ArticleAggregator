using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.Core.Repositories.Interfaces;

public interface IArticleRepository
{
    Task<Article> Get(long articleId);
    Task<int> Create(Article article);
    Task<bool> Update(Article article);
    Task<bool> Delete(long articleId);
    Task CreateMany(IList<Article> articles);
}