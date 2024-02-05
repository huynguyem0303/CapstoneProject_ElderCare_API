using AutoMapper;
using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var list=await _context.Set<Transaction>().ToListAsync();
            if (list==null) { }
            return list;

        }
    }
}
