using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAll();
        Task<Account?> GetByIdAsync(int? id);
        Task<IEnumerable<Account>> FindAsync(Expression<Func<Account, bool>> expression, params Expression<Func<Account, object>>[] includes);
        Task<Account> AddAccountAsync(SignInDto model);
        Task UpdateAccount(Account account);
        Task DeleteAccount(int id);
        Task<bool> AccountExists(int id);
        int? GetMemberIdFromToken(ClaimsPrincipal userClaims);
    }
}
