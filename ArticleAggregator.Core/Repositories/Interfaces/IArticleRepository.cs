using ArticleAggregator.Core.DataModels;
using IMilosk.Data.SqlKata.Utils.Repositories;

namespace ArticleAggregator.Core.Repositories.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Task<(int Inserted, int Updated)> UpsertMany(IEnumerable<Article> articles);
}