using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> _set;
        public Repository(AppDbContext db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");

            IQueryable<T> query = _set.AsQueryable();
            foreach (var inc in includes) query = query.Include(inc);
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
                                                      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                      int? page = null, int? pageSize = null,
                                                      bool asNoTracking = true,
                                                      params Expression<Func<T, object>>[] includes)
        {
            if (page <= 0) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));

            IQueryable<T> query = _set.AsQueryable();
            if (asNoTracking) query = query.AsNoTracking();

            if (predicate != null) query = query.Where(predicate);
            foreach (var inc in includes) query = query.Include(inc);
            if (orderBy != null) query = orderBy(query);
            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _set.AsQueryable();
            if (predicate != null) query  = query.Where(predicate);
            return await query.CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _set.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            entity.UpdatedAt = DateTime.Now;
            _set.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");

            var entity = await _set.FirstOrDefaultAsync(x=> x.Id == id);
            if (entity is null) return;
            _set.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return _set.AnyAsync(predicate);
        }

        public IQueryable<T> Query() => _set.AsQueryable();
    }
}
