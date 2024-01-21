using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWhen(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
