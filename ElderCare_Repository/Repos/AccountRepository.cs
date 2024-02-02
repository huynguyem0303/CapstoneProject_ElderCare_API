using DataAccess.Repositories;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class AccountRepository : GenericRepo<Account>, IAccountRepository
    {
        public AccountRepository(ElderCareContext context) : base(context)
        {

        }

        public async Task<Account?> LoginCarerAsync(string email, string password)
        {
            return await _context.Set<Account>().
                FirstOrDefaultAsync(x => (x.Email == email && x.Password == password)
                && x.RoleId == 4 && x.Status == (int)AccountStatus.Active);
        }

        public async Task<Account?> LoginCustomerAsync(string email, string password)
        {
            return await _context.Set<Account>().
                 FirstOrDefaultAsync(x => (x.Email == email && x.Password == password)
                 && x.RoleId == 3 && x.Status == (int)AccountStatus.Active);
        }

        public async Task<Account?> LoginStaffAsync(string email, string password)
        {
            return await _context.Set<Account>().
                 FirstOrDefaultAsync(x => (x.Email == email && x.Password == password)
                 && x.RoleId == 2 && x.Status == (int)AccountStatus.Active);
        }
        public new async Task AddAsync(Account entity)
        {
            try
            {
                entity.AccountId = _dbSet.Last().AccountId + 1;
                entity.Status = 1;
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FCMToken>?> GetFCMTokensByAccountId(int accountId)
        {
            return await _context.FCMTokens.Where(e => e.AccountId.Equals(accountId)).ToListAsync();
        }

        public async Task AddFCMToken(int accountId, string tokenValue)
        {
            var isDublicate = await _context.FCMTokens.AnyAsync(e=>e.AccountId.Equals(accountId)
                                                                   && e.Account.Status == (int)AccountStatus.Active
                                                                   && e.TokenValue == tokenValue);
            
            if (!isDublicate && !tokenValue.IsNullOrEmpty())
            {
                await _context.FCMTokens.AddAsync(new FCMToken() { AccountId = accountId, TokenValue = tokenValue });
            }
            
        }
    }
}
