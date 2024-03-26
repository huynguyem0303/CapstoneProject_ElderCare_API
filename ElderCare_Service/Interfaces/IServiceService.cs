using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface IServiceService
    {
        IEnumerable<Service> GetAll();
        Task<Service?> GetById(int id);
        Task<IEnumerable<Service>> FindAsync(Expression<Func<Service, bool>> expression, params Expression<Func<Service, object>>[] includes);
    }
}
