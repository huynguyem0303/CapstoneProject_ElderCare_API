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
    public interface ICustomerService
    {
        Task<bool> CustomerExists(int id);
        Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> expression, params Expression<Func<Customer, object>>[] includes);
        IEnumerable<Customer> GetAll();
        IEnumerable<Carer> GetCarersByCustomerId(int customerId);
        Task UpdateCustomer(UpdateCustomerDto customer);
    }
}
