using DataAccess.Repositories;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public async Task ExpriedContract()
        {
            var contract = _context.Contracts.ToList();
            for (int i=0;i<contract.Count;i++)
            {

                if (IsContractExpired(contract[i].ContractId).Result == true)
                {
                    contract[i].Status = ((int)ContractStatus.WaitingTransaction) ;
                    _dbSet.Update(contract[i]);
   
                }

            }
        }

        public async Task<List<ElderCare_Domain.Models.Contract>> GetByCarer(int id)
        {
           return _context.Contracts.Where(x=>x.CarerId==id).Include(x=>x.Elderly).Include(x => x.Carer).Include(x => x.Customer).Include(x => x.Package).ToList();
        }

        public async Task<List<ElderCare_Domain.Models.Contract>> GetByPackageIdAsync(int id)
        {
            return _context.Contracts.Where(x => x.Packageprice == id).ToList();
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

        public async Task<bool> IsContractExpired(int contractId)
        {
            var contractLastVersion = await _context.ContractVersions.Where(e => e.ContractId == contractId).OrderBy(e => e.EndDate).LastAsync();
            return contractLastVersion.EndDate < DateTime.Today;
        }

        public async Task TransactionContract()
        {

            var contract = _context.Contracts.ToList();
            for (int i = 0; i < contract.Count; i++)
            {

                if (contract[i].Status == ((int)ContractStatus.WaitingTransaction))
                {
                    contract[i].Status = ((int)ContractStatus.Expired);
                    _dbSet.Update(contract[i]);

                }

            }
        }

        public async Task<List<ContractPriceDto>> TransactionContractPrice()
        {
            List<ContractPriceDto> costlist = new List<ContractPriceDto>() ;
            var contract = _context.Contracts.ToList();
            for (int i = 0; i < contract.Count; i++)
            {
                if (contract[i].Status == ((int)ContractStatus.WaitingTransaction))
                {
                    if (contract[i].ContractType== ((int)ContractType.PackageContract))
                    {
                        ContractPriceDto cost = new ContractPriceDto();
                        cost.price = contract[i].Packageprice;
                        cost.contractId = contract[i].ContractId;
                        costlist.Add(cost);
                    }
                    else
                    {
                        ContractPriceDto cost = new ContractPriceDto();
                        double? price=0;
                        var contractservice = await _context.ContractServices.Where(e => e.ContractId == contract[i].ContractId).ToListAsync();
                        for (int j = 0; j < contractservice.Count; j++)
                        {
                            price = price+contractservice[j].Price;
                        }
                        cost.price = price;
                        cost.contractId = contract[i].ContractId;
                        costlist.Add(cost);


                    }
                    
                }
              
            }
            return costlist;


        }
    }
}
