using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace g.identity.dataAccess.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _set;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public Task<bool> Any(Expression<Func<T, bool>> predicate, CancellationToken token = default)
        => _set.AnyAsync(predicate, token);

    public Task<T> Get(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] include)
        => Get(predicate, e => e, token, include);

    public Task<TK> Get<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        return GetQueryable(predicate, true, includes).Where(predicate).Select(selector).FirstOrDefaultAsync(token);
    }

    public Task<List<T>> GetMany(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        return GetMany(predicate, e => e, token, includes);
    }

    public Task<List<TK>> GetMany<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        return GetQueryable(predicate, true, includes).Where(predicate).Select(selector).ToListAsync(cancellationToken: token);
    }

    public Task<List<TK>> GetMany<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<TK, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, true, includes).Where(predicate).Select(selector);
        if (sort != null)
            query = desc ? query.OrderByDescending(sort) : query.OrderBy(sort);

        return query.ToListAsync(cancellationToken: token);
    }

    public Task<List<TK>> GetMany<TK>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, true, includes).Where(predicate);
        if (sort != null)
            query = desc ? query.OrderByDescending(sort) : query.OrderBy(sort);

        return query.Select(selector).ToListAsync(cancellationToken: token);
    }

    public Task<(int total, List<T> data)> GetPaged(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        return GetPaged(page, count, predicate, e => e, sort, desc, token, includes);
    }

    public async Task<(int total, List<TK> data)> GetPaged<TK>(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, true, includes).Where(predicate);
        var projection = (desc ? query.OrderByDescending(sort) : query.OrderBy(sort)).Select(selector);

        var total = await projection.CountAsync(cancellationToken: token);
        var data = await projection
            .Skip(((page <= 0 ? 1 : page) - 1) * count)
            .Take(count)
            .ToListAsync(cancellationToken: token);

        return (total, data);
    }

    public Task<T> Read(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
        => Read(predicate, e => e, token, includes);

    public Task<TR> Read<TR>(
        Expression<Func<T, bool>> predicate, 
        Expression<Func<T, TR>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
        => GetQueryable(predicate, false, includes).Select(selector).FirstOrDefaultAsync(token);

    public Task<List<T>> ReadMany(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
        => ReadMany(predicate, e => e, token, includes);

    public Task<List<TR>> ReadMany<TR>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
        => GetQueryable(predicate, false, includes).Where(predicate).Select(selector).ToListAsync(token);

    public Task<List<TR>> ReadMany<TR>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, true, includes).Where(predicate);
        var projection = (sort == null ? query : desc ? query.OrderByDescending(sort) : query.OrderBy(sort))
            .Select(selector);

        return projection.ToListAsync(cancellationToken: token);
    }

    public Task<List<TR>> ReadMany<TR>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        Expression<Func<TR, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, true, includes).Where(predicate).Select(selector);
        if (sort != null)
            query = desc ? query.OrderByDescending(sort) : query.OrderBy(sort);

        return query.ToListAsync(cancellationToken: token);
    }

    public async Task<(int total, List<T> data)> ReadPaged(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, false, includes).Where(predicate);
        query = desc ? query.OrderByDescending(sort) : query.OrderBy(sort);

        var total = await query.CountAsync(cancellationToken: token);
        var data = await query
            .Skip(((page <= 0 ? 1 : page) - 1) * count)
            .Take(count)
            .ToListAsync(cancellationToken: token);

        return (total, data);
    }

    public async Task<(int total, List<TK> data)> ReadPaged<TK>(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TK>> selector,
        Expression<Func<T, object>> sort,
        bool desc = true,
        CancellationToken token = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, false, includes).Where(predicate);
        var projection = (desc ? query.OrderByDescending(sort) : query.OrderBy(sort)).Select(selector);

        var total = await projection.CountAsync(cancellationToken: token);
        var data = await projection
            .Skip(((page <= 0 ? 1 : page) - 1) * count)
            .Take(count)
            .ToListAsync(cancellationToken: token);

        return (total, data);
    }

    public async Task<(int total, List<TR> data)> ReadPagedProjection<TR>(
        int page, int count,
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, TR>> selector,
        Expression<Func<TR, object>> sort,
        bool desc = true,
        CancellationToken token = default, params Expression<Func<T, object>>[] includes)
    {
        var query = GetQueryable(predicate, false, includes).Select(selector);
        var total = await query.CountAsync(cancellationToken: token);
        var data = await (desc ? query.OrderByDescending(sort) : query.OrderBy(sort))
            .Skip(((page <= 0 ? 1 : page) - 1) * count)
            .Take(count)
            .ToListAsync(cancellationToken: token);

        return (total, data);
    }

    public Task<int> Count(
        Expression<Func<T, bool>> predicate,
        CancellationToken token = default)
        => _set.CountAsync(predicate, token);

    public T Add(T entity)
    {
        _set.Add(entity);
        return entity;
    }

    public List<T> Add(List<T> entities)
    {
        _set.AddRange(entities);
        return entities;
    }

    public async Task<T> AddAndSave(T entity, CancellationToken token = default)
    {
        _set.Add(entity);
        await _context.SaveChangesAsync(token);
        return entity;
    }

    public async Task<List<T>> AddAndSave(List<T> entities, CancellationToken token = default)
    {
        _set.AddRange(entities);
        await _context.SaveChangesAsync(token);
        return entities;
    }

    public T Update(T entity)
    {
        _context.Update(entity);
        return entity;
    }

    public List<T> Update(List<T> entities)
    {
        _context.UpdateRange(entities);
        return entities;
    }

    public async Task<T> UpdateAndSave(T entity, CancellationToken token = default)
    {
        var attached = await LookupAttached(entity, token);
        if (attached is { })
            return attached;

        var result = _set.Update(entity);
        if (result.State != EntityState.Modified)
            return null;

        await _context.SaveChangesAsync(token);
        result.State = EntityState.Detached;
        return result.Entity;
    }

    public async Task<List<T>> UpdateAndSave(List<T> entities, CancellationToken token = default)
    {
        _set.UpdateRange(entities);
        await _context.SaveChangesAsync(token);
        return entities;
    }

    public void Remove(T entity)
    {
        _context.Remove(entity);
    }

    public void Remove(IEnumerable<T> entities)
    {
        _context.RemoveRange(entities);
    }

    public async Task<bool> RemoveAndSave(T entity, CancellationToken token = default)
    {
        _set.Remove(entity);
        return await _context.SaveChangesAsync(token) > 0;
    }

    public async Task<bool> RemoveAndSave(IEnumerable<T> entities, CancellationToken token = default)
    {
        _set.RemoveRange(entities);
        return await _context.SaveChangesAsync(token) > 0;
    }

    private IQueryable<T> GetQueryable(
        Expression<Func<T, bool>> predicate,
        bool tracking,
        params Expression<Func<T, object>>[] include)
    {
        var query = (tracking ? _set.AsQueryable() : _set.AsQueryable().AsNoTracking())
            .AsSplitQuery().Where(predicate);
        
        if (include != null && include.Any())
            query = include.Aggregate(query, (current, inc) => current.Include(inc));

        return query;
    }

    public async Task<bool> Save(CancellationToken token = default)
        => await _context.SaveChangesAsync(token) > 0;

    private async Task<T> LookupAttached(T entity, CancellationToken token = default)
    {
        var attached = _set.Local.FirstOrDefault(e => e == entity);
        if (attached is null)
        {
            return null;
        }

        var type = typeof(T);
        var entry = _context.Entry(attached);
        entry.Members.Where(m => m.Metadata.PropertyInfo?.CanWrite ?? false).ToList().ForEach(e =>
        {
            
            var updateValue = type.GetProperty(e.Metadata.Name)?.GetValue(entity);
            if (e.CurrentValue != null && e.CurrentValue.Equals(updateValue))
                return;

            e.CurrentValue = updateValue;
            e.IsModified = true;
        });

        await _context.SaveChangesAsync(token);
        entry.State = EntityState.Detached;
        return attached;
    }
}