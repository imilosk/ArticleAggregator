using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.Core.Repositories.Interfaces;

public interface IArticleRepository
{
    Task<Article?> Get(long articleId);
    Task<bool> Exists(long articleId);
    Task<bool> Exists(Uri link);
    Task<int> Create(Article article);
    Task<int> Update(Article article);
    Task<int> Delete(long articleId);
    Task<int> UpsertMany(IList<Article> articles);
}