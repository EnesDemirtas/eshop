using Eshop.DataAccess.Repository.IRepository;
using Eshop.DataAccess.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Eshop.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class {
    private readonly ApplicationDbContext _context;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext context) {
        _context = context;
        this.dbSet = _context.Set<T>();
        _context.Products.Include(p => p.Category).Include(c => c.CategoryId);
    }

    public void Add(T entity) {
        dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null) {
        IQueryable<T> query = dbSet;
        if (filter != null) query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties)) {
            foreach (var prop in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(prop);
            }
        }
        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null) {
        IQueryable<T> query = dbSet;
        if (filter != null) query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties)) {
            foreach (var prop in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(prop);
            }
        }
        return query.ToList();
    }

    public void Remove(T entity) {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities) {
        dbSet.RemoveRange(entities);
    }
}