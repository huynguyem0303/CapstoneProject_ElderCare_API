using DataAccess.Repositories;
using ElderCare_Domain.Enums;
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
    public class CustomerRepository : GenericRepo<Customer>, ICustomerRepository
    {
        public CustomerRepository(ElderCareContext context) : base(context)
        {

        }

        public new async Task<Customer> AddAsync(Customer entity)
        {
            try
            {
                //CheckIfNullException();
                entity.CustomerId = _dbSet.OrderBy(e => e.CustomerId).Last().CustomerId + 1;
                entity.Status = (int)AccountStatus.Active;
                if (entity.Bankinfo == null)
                {
                    throw new Exception("Missing Bank Info");
                }
                entity.Bankinfo.BankinfoId = _context.Bankinformations.OrderBy(e => e.BankinfoId).Last().BankinfoId + 1;
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This account has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return entity;
        }
        private void CheckIfNullException()
        {
            _ = _dbSet.Select(e => e.CustomerName).ToList();
            _ = _dbSet.Select(e => e.Email).ToList();
            _ = _dbSet.Select(e => e.Phone).ToList();
            _ = _dbSet.Select(e => e.Status).ToList();
            _ = _dbSet.Select(e => e.BankinfoId).ToList();
            //_ = _dbSet.Select(e => e.TransactionId).ToList();
            _ = _dbSet.Select(e => e.Bankinfo).ToList();
            _ = _context.Bankinformations.Select(e => e.BankinfoId).ToList();
            _ = _context.Bankinformations.Select(e => e.BankAccountName).ToList();
            _ = _context.Bankinformations.Select(e => e.BankAccountNumber).ToList();
            _ = _context.Bankinformations.Select(e => e.BankName).ToList();
            _ = _context.Bankinformations.Select(e => e.Branch).ToList();
        }
    }
}
