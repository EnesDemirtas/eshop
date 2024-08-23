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
    }

    public void Add(T entity) {
        dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter) {
        IQueryable<T> query = dbSet;
        T result = query.FirstOrDefault(filter);
        return result;
    }

    public IEnumerable<T> GetAll() {
        IQueryable<T> query = dbSet;
        return query.ToList();
    }

    public void Remove(T entity) {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities) {
        dbSet.RemoveRange(entities);
    }
}