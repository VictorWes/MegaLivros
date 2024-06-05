using MegaLivros.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MegaLivros.Repositories;

public class Repositore<T> : IRepositore<T> where T : class
{
    protected readonly MegaLivrosContext _context;

    public Repositore(MegaLivrosContext context)
    {
        _context = context;
    }
    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();

    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
       return await _context.Set<T>().FirstOrDefaultAsync(predicate);
       
    }

    public T Update(T entity)
    {
       _context.Set<T>().Update(entity);
      
        return entity;
    }
}
