using UserAuthentication.Data;
using UserAuthentication.Entities;
using UserAuthentication.Interface;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace UserAuthentication.Services
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly UserAuthenticationContext _dbContext;

        public EFRepository(UserAuthenticationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T entity)
        {
            try
            {
                await _dbContext.Set<T>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           

        }

        public async Task<T> FetchRecord(Expression<Func<T, bool>> predicate)
        {

            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();

        }

    }
}
