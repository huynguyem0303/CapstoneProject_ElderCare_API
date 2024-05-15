using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface IPackageRepo:IGenericRepo<Package>
    {
        public Task<List<PackageService>> AddPackageService(int packageId, string[] serviceName);
        Task RemovePackageService(int packageId, int serviceId);
        Task<bool> PackageServiceExisted(int packageId);
        Task<List<Package>> GetByCarerId(int id);
    }
}
