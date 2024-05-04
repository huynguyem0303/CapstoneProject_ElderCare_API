using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface IContractRepository : IGenericRepo<Contract>
    {
        Task AddContractVersionAsync(DateTime startDate, DateTime endDate,int contractid);
        Task AddContractServiceAsync(string[] service, int contractid);
        Task<Package> GetPackageAsync(string name);
        Task<Double?> GetPackagePrice();
        Task<List<Contract>> GetByPackageIdAsync(int id);
        Task<List<Contract>> GetByCarer(int id);
        Task<bool> IsContractExpired(int contractId);
    }
}
