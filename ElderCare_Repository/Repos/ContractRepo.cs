using DataAccess.Repositories;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class ContractRepo : GenericRepo<ElderCare_Domain.Models.Contract>, IContractRepository
    {
        private double? price;
        public ContractRepo(ElderCareContext context) : base(context)
        {

        }

        public async Task AddContractServiceAsync(string[] service, int contractid)
        {
            price = 0;
            try
            {

                var entity = new ContractService();
                for (int i = 0; i < service.Length; i++)
                {
                    entity.ContractServicesId = _context.ContractServices.OrderBy(x => x.ContractServicesId).Last().ContractServicesId + 1;
                    entity.ServiceId = _context.Services.FirstOrDefault(x => x.Name.Equals(service[i])).ServiceId;
                    entity.Price = _context.Services.FirstOrDefault(x => x.Name.Equals(service[i])).Price;
                    entity.ContractId = contractid;
                    await _context.ContractServices.AddAsync(entity);
                }

            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This entity has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task AddContractVersionAsync(DateTime startDate, DateTime endDate, int contractid)
        {
            try
            {
                var entity = new ContractVersion();
                entity.ContractVersionId = _context.ContractVersions.OrderBy(x => x.ContractVersionId).Last().ContractVersionId + 1;
                entity.StartDate = startDate;
                entity.EndDate = endDate;
                entity.CreatedDate = DateTime.Now;
                entity.Status = 1;
                entity.ContractId = contractid;
                await _context.ContractVersions.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This entity has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ElderCare_Domain.Models.Contract>> GetByCarer(int id)
        {
           return _context.Contracts.Where(x=>x.CarerId==id && x.Status==(int)ContractStatus.Pending).Include(x=>x.Elderly).Include(x => x.Carer).Include(x => x.Customer).Include(x => x.Package).ToList();
        }

        public async Task<Package> GetPackageAsync(string name)
        {
            price = 0;
            var package = _context.Packages.FirstOrDefaultAsync(x => x.Name.Equals(name)).Result;
            price = package?.Price;
            return package;
        }

        public async Task<double?> GetPackagePrice()
        {
            return price;
        }
    }
}
