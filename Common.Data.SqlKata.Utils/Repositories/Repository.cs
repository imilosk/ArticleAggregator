using System.Data;
using Common.Data.SqlClient;
using Common.Data.SqlKata.Utils.Extensions;
using Common.Data.SqlKata.Utils.Filtering;
using Dapper;
using SqlKata.Execution;

namespace Common.Data.SqlKata.Utils.Repositories;

public abstract class Repository<T> : IRepository<T>
{
    private readonly QueryFactory _queryFactory;
    private readonly string _tableName;
    private readonly string _insertSql;

    protected Repository(string tableName, QueryFactory queryFactory)
    {
        _queryFactory = queryFactory;

        var columns = SqlDataMapper.GetColumnNames<T>();
        _insertSql = GenerateInsertStatement(tableName, columns);
        _tableName = tableName;
    }

    public async Task<T?> Get(QueryFilter queryFilter, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilter(queryFilter)
            .FirstOrDefaultAsync<T>(transaction);
    }

    public async Task<T?> Get(IEnumerable<QueryFilter> queryFilters, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilters(queryFilters)
            .FirstOrDefaultAsync<T>(transaction);
    }

    public async Task<IEnumerable<T>> GetAll(IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .GetAsync<T>(transaction);
    }

    public async Task<IEnumerable<T>> GetAll(QueryFilter queryFilter, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilter(queryFilter)
            .GetAsync<T>(transaction);
    }

    public async Task<IEnumerable<T>> GetAll(
        IEnumerable<QueryFilter> queryFilters,
        IDbTransaction? transaction = null
    )
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilters(queryFilters)
            .GetAsync<T>(transaction);
    }

    public async Task<bool> Exists(QueryFilter queryFilter, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilter(queryFilter)
            .ExistsAsync(transaction);
    }

    public async Task<bool> Exists(IEnumerable<QueryFilter> queryFilter, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilters(queryFilter)
            .ExistsAsync(transaction);
    }

    public async Task<int> Create(T entity, IDbTransaction? transaction = null)
    {
        return await _queryFactory.Connection.ExecuteAsync(_insertSql, entity, transaction);
    }

    public async Task<int> Update(
        T entity,
        QueryFilter queryFilter,
        IDbTransaction? transaction = null
    )
    {
        var updateObject = SqlDataMapper.GetAnonymousObject(entity);

        return await _queryFactory
            .Query(_tableName)
            .ApplyFilter(queryFilter)
            .UpdateAsync(updateObject, transaction);
    }

    public async Task<int> Update(
        T entity,
        IEnumerable<QueryFilter> queryFilter,
        IDbTransaction? transaction = null
    )
    {
        var updateObject = SqlDataMapper.GetAnonymousObject(entity);

        return await _queryFactory
            .Query(_tableName)
            .ApplyFilters(queryFilter)
            .UpdateAsync(updateObject, transaction);
    }

    public async Task<int> Delete(QueryFilter queryFilter, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilter(queryFilter)
            .DeleteAsync(transaction);
    }

    public async Task<int> Delete(IEnumerable<QueryFilter> queryFilters, IDbTransaction? transaction = null)
    {
        return await _queryFactory
            .Query(_tableName)
            .ApplyFilters(queryFilters)
            .DeleteAsync(transaction);
    }

    private static string GenerateInsertStatement(string tableName, IList<string> columns)
    {
        var columnsString = string.Join(",", columns);
        var parametersString = string.Join(", ", columns.Select(c => "@" + c));

        return $"INSERT INTO {tableName} ({columnsString}) VALUES ({parametersString})";
    }
}