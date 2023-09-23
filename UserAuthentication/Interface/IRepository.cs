using System.Linq.Expressions;
using UserAuthentication.Entities;

namespace UserAuthentication.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        //Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);

        Task<T> FetchRecord(Expression<Func<T, bool>> predicate);
        //Task Update(T entity);
    }
}
