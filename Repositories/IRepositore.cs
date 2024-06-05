using System.Linq.Expressions;

namespace MegaLivros.Repositories;

public interface IRepositore<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Delete(T entity);
    T Update(T entity);
}
