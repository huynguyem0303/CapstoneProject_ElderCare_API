using ElderCare_Domain.Commons;
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
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task<Pagination<T>> ToPagination(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<T>> ToPagination(Expression<Func<T, bool>> expression, int pageIndex = 0, int pageSize = 10);
        Task<Pagination<T>> ToPagination(IQueryable<T> value, Expression<Func<T, bool>> expression, int pageIndex, int pageSize);
    }
}
