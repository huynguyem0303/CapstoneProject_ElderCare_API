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
    public interface IContractService
    {
        
        Task<Contract> AddContract(AddContractDto dto);
        Task<IEnumerable<Contract>> FindAsync(Expression<Func<Contract, bool>> expression, params Expression<Func<Contract, object>>[] includes);
        Task<List<Contract>> GetByCarerId(int id);
        Task<Contract?> ApproveContract(int contractid, int status);
        Task ExpriedContract();

        Task<bool> ContractExists(int id);
        Task<List<Contract>> ExpriedContractToday();
        Task<List<Contract>> ExpriedContractInNext5Day();
        Task<(Contract, List<Timetable>)> AddContract2(AddContractWithTrackingsDto dto);
    }
}
