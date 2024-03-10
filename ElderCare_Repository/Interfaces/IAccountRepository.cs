using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepo<Account>
    {
        Task<Account?> LoginCustomerAsync(String email, String password);
        Task<Account?> LoginStaffAsync(String email, String password);
        Task<Account?> LoginCarerAsync(String email, String password);
        Task<List<Device>?> GetFCMTokensByAccountId(int accountId);
        Task AddFCMToken(int accountId, string tokenValue);
        int? GetMemberIdFromToken(ClaimsPrincipal userClaims);
    }
}
