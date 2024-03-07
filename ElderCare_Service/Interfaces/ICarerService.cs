using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface ICarerService
    {
        IEnumerable<Carer> GetAll();
        Task<Carer?> GetById(int id);
        Task<IEnumerable<Carer>> FindAsync(Expression<Func<Carer, bool>> expression, params Expression<Func<Carer, object>>[] includes);
        //Task<Carer> AddCarerAsync(Carer model);
        Task UpdateCarer(Carer Carer);
        Task DeleteCarer(int id);
        Task<bool> CarerExists(int id);
        Task<List<Carer>?> SearchCarer(SearchCarerDto dto);
       
        Task<List<CarerTransactionDto>> GetCarerTransactionHistoryAsyncByCarerId(int carerId);
        Task<List<CarerTransactionDto>> GetCarerTransactionHistoryAsyncByCustomerId(int customerId);
    }
}
