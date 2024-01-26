using DataAccess.Interfaces;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected readonly ElderCareContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepo(ElderCareContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<T?> GetByIdAsync(object id)
        {
            try
            {
                var item = await _dbSet.FindAsync(id);
                return item!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message:"This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task<Pagination<T>> ToPagination(int pageIndex = 0, int pageSize = 10)
            => ToPagination(x => true, pageIndex, pageSize);
        public Task<Pagination<T>> ToPagination(Expression<Func<T, bool>> expression, int pageIndex = 0, int pageSize = 10)
            => ToPagination(_dbSet, expression, pageIndex, pageSize);
        public async Task<Pagination<T>> ToPagination(IQueryable<T> value, Expression<Func<T, bool>> expression, int pageIndex, int pageSize)
        {

            var itemCount = await value.Where(expression).CountAsync();
            var items = await value.Where(expression)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            return await includes
                .Aggregate(_dbSet.AsQueryable(),
                (entity, property) => entity.Include(property))
                .Where(expression).ToListAsync();
        }
    }
}
