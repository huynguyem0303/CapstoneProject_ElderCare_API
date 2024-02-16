using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using ElderCare_Repository.Services;
using Microsoft.AspNetCore.Http;
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
        private readonly INotificationService _notificationService;
        private readonly ITransactionRepo _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IElderRepo _elderRepo;

        public UnitOfWork(ElderCareContext context, IAccountRepository accountRepository, ICustomerRepository customerRepository, ICarerRepository carerRepository, INotificationService notificationService, ITransactionRepo transactionRepository, IHttpContextAccessor httpContextAccessor, IElderRepo elderRepo)
        {
            _context = context;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _carerRepository = carerRepository;
            _notificationService = notificationService;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
            _elderRepo = elderRepo;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public ICustomerRepository CustomerRepository => _customerRepository;
        public ICarerRepository CarerRepository => _carerRepository;
        public ITransactionRepo TransactionRepo => _transactionRepository;
 
        public INotificationService NotificationService => _notificationService;

        public IElderRepo ElderRepo => _elderRepo;
        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
