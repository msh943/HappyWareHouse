using HappyWarehouse.Domain.Entities;
using System.Linq.Expressions;

namespace HappyWarehouse.Infrastructure.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<TOut>> GetAllLookupsAsync<TOut>(
                                   Expression<Func<T, TOut>> selector,
                                   bool asNoTracking = true,
                                   CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        int? page = null, int? pageSize = null,
                                        bool asNoTracking = true,
                                        params Expression<Func<T, object>>[] includes);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
    }
}
