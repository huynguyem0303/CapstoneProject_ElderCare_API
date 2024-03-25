using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface IPackageService
    {
        IEnumerable<Package> GetAll();
        Task<Package?> GetById(int id);
        Task<IEnumerable<Package>> FindAsync(Expression<Func<Package, bool>> expression, params Expression<Func<Package, object>>[] includes);
    }
}
