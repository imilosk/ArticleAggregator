using System.Data;
using Common.Data.SqlKata.Utils.Filtering;

namespace Common.Data.SqlKata.Utils.Repositories;

public interface IRepository<T>
{
    Task<T?> Get(QueryFilter queryFilter, IDbTransaction? transaction = null);
    Task<T?> Get(IEnumerable<QueryFilter> queryFilters, IDbTransaction? transaction = null);
    Task<IEnumerable<T>> GetAll(IDbTransaction? transaction = null);
    Task<IEnumerable<T>> GetAll(QueryFilter queryFilter, IDbTransaction? transaction = null);
    Task<IEnumerable<T>> GetAll(IEnumerable<QueryFilter> queryFilters, IDbTransaction? transaction = null);
    Task<bool> Exists(QueryFilter queryFilter, IDbTransaction? transaction = null);
    Task<bool> Exists(IEnumerable<QueryFilter> queryFilter, IDbTransaction? transaction = null);
    Task<int> Create(T entity, IDbTransaction? transaction = null);
    Task<int> Update(T entity, QueryFilter queryFilter, IDbTransaction? transaction = null);
    Task<int> Update(T entity, IEnumerable<QueryFilter> queryFilter, IDbTransaction? transaction = null);
    Task<int> Delete(QueryFilter queryFilter, IDbTransaction? transaction = null);
    Task<int> Delete(IEnumerable<QueryFilter> queryFilters, IDbTransaction? transaction = null);
}