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
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarerRepository _carerRepository;

        public UnitOfWork(ElderCareContext context, IAccountRepository accountRepository, ICustomerRepository customerRepository, ICarerRepository carerRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _carerRepository = carerRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public ICustomerRepository CustomerRepository => _customerRepository;
        public ICarerRepository CarerRepository => _carerRepository;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
