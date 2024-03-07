using AutoMapper;
using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class TransactionRepo : GenericRepo<Transaction>, ITransactionRepo
    {
        private readonly IMapper _mapper;
        public TransactionRepo(ElderCareContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            var list = await _context.Set<Transaction>().ToListAsync();
            if (list == null) { }
            return list;

        }

        public async Task<Transaction> GetLastestTransaction(int id)
        {
            var result = await _context.Set<Transaction>().Where(t => t.AccountId == id)
     .OrderByDescending(t => t.TransactionId)
     .FirstOrDefaultAsync();
            return result;
        }

        public async Task<Transaction> GetTransaction(long id)
        {
            var result = await _context.Set<Transaction>().Where(t => t.TransactionId == id).FirstOrDefaultAsync();
            return result;
        }

        

        public async Task UpdateOrderInfoInDatabase(Transaction transaction)
        {
            _context.Entry(transaction).State = EntityState.Modified;
            _context.SaveChangesAsync();
        }


        public async Task<List<Transaction>> GetCarerTransaction(int carerId)
        {
            var carerCusIdList = await _context.CarersCustomers.Where(x => x.CarerId == carerId).Select(x => x.CarercusId).ToListAsync();
            if (carerCusIdList.IsNullOrEmpty())
            {
                throw new Exception("Empty transaction history");
            }
            var transactionList = await _context.Transactions.Where(x => carerCusIdList.Contains((int)x.CarercusId!)).ToListAsync();
            return transactionList;
        }
        public async Task<List<Transaction>> GetCustomerTransaction(int customerId)
        {
            var carerCusIdList = await _context.CarersCustomers.Where(x => x.CustomerId == customerId).Select(x => x.CustomerId).ToListAsync();
            if (carerCusIdList.IsNullOrEmpty())
            {
                throw new Exception("Empty transaction history");
            }
            var transactionList = await _context.Transactions.Where(x => carerCusIdList.Contains((int)x.CarercusId!)).ToListAsync();
            return transactionList;
        }
    }
}
