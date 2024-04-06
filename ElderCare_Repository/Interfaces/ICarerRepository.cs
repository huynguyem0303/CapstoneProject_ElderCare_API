
using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface ICarerRepository : IGenericRepo<Carer>
    {
        Task<List<Carer>?> searchCarer(SearchCarerDto dto );
        new Task<Carer> AddAsync(Carer entity);
        new Task<CarersCustomer> AddCarerCusAsync(CarersCustomer entity);
        Task<CarersCustomer?> FindAsync(int carerid, int cusid);
        Task<List<Carer>> GetPendingCarerAsync();
        Task<List<Transaction>> GetCarerTransaction(int carerId);
        Task<List<Transaction>> GetCustomerTransaction(int customerId);
        Task<CarersCustomer?> GetCarerCustomerFromIdAsync(int? carercusId);

        Task<CarersCustomer?> GetLastest();
        Task<List<CarerService>> AddCarerService(int carerId, string[] serviceName);
        Task RemoveCarerService(int carerId, int serviceId);
        Task<CarerService?> GetCarerService(int carerId, int serviceId);
        IEnumerable<Carer> GetCarersByCustomerId(int customerId);
    }
}
