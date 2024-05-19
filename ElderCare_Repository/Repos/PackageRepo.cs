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
            int lastId = (_context.PackageServices.OrderBy(e => e.PackageServicesId).Last() ?? new PackageService()).PackageServicesId;
            for (var i = 0; i < serviceName.Length; i++)
            {
                var service = await _context.Services.FirstOrDefaultAsync(e => e.Name == serviceName[i]);
                if (service == null)
                {
                    errors.Add($"serviceName[{i}]:'{serviceName[i]}' is incorrect");
                }
                else
                {
                    if (_context.PackageServices.Where(e => e.PackageId == packageId && e.ServiceId == service.ServiceId).Any())
                    {
                        errors.Add($"serviceName[{i}]:'{serviceName[i]}' existed in the package");
                    }
                    else
                    {
                        list.Add(new PackageService()
                        {
                            PackageServicesId = ++lastId,
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
            list = list.DistinctBy(e => e.ServiceId).ToList();
            if (list.Count > 0)
            {
                await _context.PackageServices.AddRangeAsync(list);
            }
            return list;
        }

        public async Task<Package> GetPackageByName(string name)
        {

            return await _context.Packages.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();

        }

        public async Task<List<Package>> GetByCarerId(int id)
        {
            var carerServices = await _context.CarerServices.Where(e => e.CarerId == id).Select(e => e.ServiceId).ToListAsync();
            var packages = _context.Packages.Include(e => e.PackageServices);
            var result = new List<Package>();
            foreach (var package in packages)
            {
                var packageServices = from packageService in package.PackageServices
                                      select packageService.ServiceId;
                if (!packageServices.Except(carerServices).Any())
                {
                    result.Add(package);
                }
            }
            return result;
        }

        public async Task<bool> PackageServiceExisted(int packageId)
        {
            var check = await _context.PackageServices.Where(x => x.PackageId == packageId).FirstOrDefaultAsync();
            if (check != null)
            {
                return true;
            }

            return false;

        }

        public async Task RemovePackageService(int packageId, int serviceId)
        {
            var packageService = await _context.PackageServices.FirstOrDefaultAsync(e => e.ServiceId == serviceId && e.PackageId == packageId);
            if (packageService != null)
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
