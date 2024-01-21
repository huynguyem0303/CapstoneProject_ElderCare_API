using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                && x.RoleId == 4);
        }

        public async Task<Account?> LoginCustomerAsync(string email, string password)
        {
            return await _context.Set<Account>().
                 FirstOrDefaultAsync(x => (x.Email == email && x.Password == password)
                 && x.RoleId == 3);
        }

        public async Task<Account?> LoginStaffAsync(string email, string password)
        {
            return await _context.Set<Account>().
                 FirstOrDefaultAsync(x => (x.Email == email && x.Password == password)
                 && x.RoleId == 2);
        }
    }
}
