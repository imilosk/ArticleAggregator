using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.Core.Repositories.Interfaces;

public interface IArticleRepository
{
    Task<Article?> Get(long articleId);
    Task<bool> Exists(long articleId);
    Task<bool> Exists(Uri link);
    Task<int> Create(Article article);
    Task<bool> Update(Article article);
    Task<bool> Delete(long articleId);
    Task UpsertMany(IList<Article> articles);
}