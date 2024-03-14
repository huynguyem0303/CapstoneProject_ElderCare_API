using DataAccess.Repositories;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
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
            if (await IsEmailDublicate(entity.Email))
            {
                throw new DuplicateNameException("This email has already been registered");
            }
            try
            {
                //CheckIfNullException();
                entity.AccountId = _dbSet.OrderBy(e=>e.AccountId).Last().AccountId + 1;
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
        public new void Delete(Account entity)
        {
            try
            {
                entity.Status = (int)AccountStatus.InActive;
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Device>?> GetFCMTokensByAccountId(int accountId)
        {
            return await _context.Devices.Where(e => e.AccountId.Equals(accountId)).ToListAsync();
        }

        public async Task AddFCMToken(int accountId, string tokenValue)
        {
            var isDublicate = await _context.Devices.AnyAsync(e => e.AccountId.Equals(accountId)
                                                                   && e.Account.Status == (int)AccountStatus.Active
                                                                   && e.DeviceFcmToken == tokenValue);

            if (!isDublicate && !tokenValue.IsNullOrEmpty())
            {
                await _context.Devices.AddAsync(new Device() { AccountId = accountId, DeviceFCMToken = tokenValue });
            }

        }

        public int? GetMemberIdFromToken(ClaimsPrincipal userClaims)
        {
            // Kiểm tra xem userClaims có tồn tại không
            if (userClaims == null)
            {
                return null;
            }

            // Tìm claim có tên là "id"
            var idClaim = userClaims.Claims.FirstOrDefault(c => c.Type == "Id");

            if (idClaim != null)
            {
                // Lấy giá trị của claim "id" và chuyển đổi thành Guid
                if (int.TryParse(idClaim.Value, out int memberId))
                {
                    return memberId;
                }
            }

            return null; // Trả về null nếu không tìm thấy hoặc không thể chuyển đổi thành Guid
        }

        private async Task<bool> IsEmailDublicate(string email)
        {
            return await _dbSet.AnyAsync(e => e.Email == email.Trim());
        }

        private void CheckIfNullException()
        {
            _ = _dbSet.Select(e => e.RoleId).ToList();
            _ = _dbSet.Select(e => e.AccountId).ToList();
            _ = _dbSet.Select(e => e.Username).ToList();
            _ = _dbSet.Select(e => e.Password).ToList();
            _ = _dbSet.Select(e => e.Email).ToList();
            _ = _dbSet.Select(e => e.PhoneNumber).ToList();
            _ = _dbSet.Select(e => e.Address).ToList();
            _ = _dbSet.Select(e => e.Status).ToList();
            _ = _dbSet.Select(e => e.CustomerId).ToList();
            _ = _dbSet.Select(e => e.CarerId).ToList();
            
        }
    }
}