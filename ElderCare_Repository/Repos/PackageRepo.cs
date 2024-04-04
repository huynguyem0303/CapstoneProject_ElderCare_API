using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class PackageRepo : GenericRepo<Package>, IPackageRepo
    {
        public PackageRepo(ElderCareContext context) : base(context)
        {
        }

        public async Task<List<PackageService>> AddPackageService(int packageId, string[] serviceName)
        {
            var errors = new List<string>();
            var list = new List<PackageService>();
            for (var i = 0; i < serviceName.Length; i++)
            {
                var service = await _context.Services.FirstOrDefaultAsync(e => e.Name == serviceName[i]);
                if(service == null)
                {
                    errors.Add($"serviceName[{i}]:'{serviceName[i]}' is incorrect");
                }
                else
                {
                    if(_context.PackageServices.Where(e => e.PackageId == packageId && e.ServiceId == service.ServiceId).Any())
                    {
                        errors.Add($"serviceName[{i}]:'{serviceName[i]}' existed in the package");
                    }
                    else
                    {
                        list.Add(new PackageService()
                        {
                            PackageServicesId = _context.PackageServices.OrderBy(e => e.PackageServicesId).Last().PackageServicesId + 1,
                            ServiceId = service.ServiceId,
                            PackageId = packageId,
                        });
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new DbUpdateException(message: String.Join(",\n", errors));
            }
            if(list.Count > 0)
            {
                await _context.PackageServices.AddRangeAsync(list);
            }
            return list;
        }

        public async Task RemovePackageService(int packageId, int serviceId)
        {
            var packageService = await _context.PackageServices.FirstOrDefaultAsync(e => e.ServiceId == serviceId && e.PackageId == packageId);
            if(packageService != null)
            {
                _context.PackageServices.Remove(packageService);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
