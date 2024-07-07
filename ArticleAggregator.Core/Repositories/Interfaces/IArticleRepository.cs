using System.Data;
using ArticleAggregator.Core.DataModels;

namespace ArticleAggregator.Core.Repositories.Interfaces;

public interface IArticleRepository
{
    Task<Article?> Get(long articleId);
    Task<Article?> Get(Uri link);
    Task<bool> Exists(long articleId);
    Task<bool> Exists(Article article);
    Task<int> Create(Article article, IDbTransaction? transaction = null);
    Task<int> Update(Article article, IDbTransaction? transaction = null);
    Task<int> Delete(long articleId);
    Task<int> UpsertMany(IList<Article> articles);
}