using ArticleAggregator.Core.DataModels;
using Common.Data.SqlKata.Utils.Repositories;

namespace ArticleAggregator.Core.Repositories.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Task<int> UpsertMany(IEnumerable<Article> articles);
}