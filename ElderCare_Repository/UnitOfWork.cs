using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ElderCareContext _context;
        private readonly IAccountRepository _accountRepository;

        public UnitOfWork(ElderCareContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
