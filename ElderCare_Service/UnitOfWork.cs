﻿using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ElderCare_Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ElderCareContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarerRepository _carerRepository;
        private readonly Interfaces.INotificationService _notificationService;
        private readonly ITransactionRepo _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IElderRepo _elderRepo;
        private readonly Interfaces.IEmailService _emailService;

        public UnitOfWork(ElderCareContext context, IAccountRepository accountRepository, ICustomerRepository customerRepository, ICarerRepository carerRepository, Interfaces.INotificationService notificationService, ITransactionRepo transactionRepository, IHttpContextAccessor httpContextAccessor, IElderRepo elderRepo, Interfaces.IEmailService emailService)
        {
            _context = context;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _carerRepository = carerRepository;
            _notificationService = notificationService;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
            _elderRepo = elderRepo;
            _emailService = emailService;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public ICustomerRepository CustomerRepository => _customerRepository;
        public ICarerRepository CarerRepository => _carerRepository;
        public ITransactionRepo TransactionRepo => _transactionRepository;
 
        public Interfaces.INotificationService NotificationService => _notificationService;

        public IElderRepo ElderRepo => _elderRepo;

        public Interfaces.IEmailService emailService => _emailService;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
