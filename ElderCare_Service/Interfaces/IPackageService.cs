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
    public interface IPackageService
    {
        Task<IEnumerable<PackageDto>> GetAllAsync();
        Task<PackageDto?> GetById(int id);
        Task<IEnumerable<Package>> FindAsync(Expression<Func<Package, bool>> expression, params Expression<Func<Package, object>>[] includes);
        Task<Package> AddPackageAsync(AddPackageDto model);
        Task UpdatePackage(UpdatePackageDto model);
        Task DeletePackage(int id);
        Task<bool> PackageExists(int id);
        Task<List<PackageServiceDto>> AddPackageServiceAsync(int packageId, string[] serviceName);
        Task RemoveServiceFromPackage(int packageId,int serviceId);
    }
}
