using System.Linq.Expressions;

namespace g.identity.dataAccess.Repositories;

public interface IBaseRepository<T> where T : class
{
    /// single element
    Task<bool> Any(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    // Get - tracking
    Task<T> Get(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] include);

    Task<TK> Get<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    /// multiple elements
    // Get - tracking
    Task<List<T>> GetMany(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<List<TK>> GetMany<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<(int total, List<T> data)> GetPaged(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<(int total, List<TK> data)> GetPaged<TK>(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    /// read - no tracking

    // single
    Task<T> Read(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<TK> Read<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    // many
    Task<List<T>> ReadMany(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<List<TR>> ReadMany<TR>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<List<TR>> ReadMany<TR>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<List<TR>> ReadMany<TR>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        Expression<Func<TR, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<(int total, List<T> data)> ReadPaged(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<(int total, List<TK> data)> ReadPaged<TK>(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<(int total, List<TK> data)> ReadPagedProjection<TK>(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<TK, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes);

    Task<int> Count(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    // write
    T Add(T entity);
    List<T> Add(List<T> entities);
    Task<T> AddAndSave(T entity, CancellationToken token = default);
    Task<List<T>> AddAndSave(List<T> entities, CancellationToken token = default);

    T Update(T entity);
    List<T> Update(List<T> entities);
    Task<T> UpdateAndSave(T entity, CancellationToken token = default);
    Task<List<T>> UpdateAndSave(List<T> entities, CancellationToken token = default);

    void Remove(T entity);
    void Remove(IEnumerable<T> entities);
    public Task<bool> RemoveAndSave(T entity, CancellationToken token = default);
    public Task<bool> RemoveAndSave(IEnumerable<T> entities, CancellationToken token = default);

    Task<bool> Save(CancellationToken token = default);
}
