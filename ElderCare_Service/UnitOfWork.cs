using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using ElderCare_Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ElderCare_Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ElderCareContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarerRepository _carerRepository;
        private readonly Interfaces.INotificationService _notificationService;
        private readonly ITransactionRepo _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IElderRepo _elderRepo;
        private readonly Interfaces.IEmailService _emailService;
        private readonly IHobbyRepo _hobbyRepo;
        private readonly IPsychomotorHealthRepo _psychomotorHealthRepo;
        private readonly IHealthDetailRepo _healthDetailRepo;
        private readonly IPsychomotorRepo _psychomotorRepo;
        private readonly IServiceRepo _serviceRepo;
        private readonly IPackageRepo _packageRepo;

        public UnitOfWork(ElderCareContext context, IAccountRepository accountRepository, IContractRepository contractRepository, ICustomerRepository customerRepository, ICarerRepository carerRepository, INotificationService notificationService, ITransactionRepo transactionRepository, IHttpContextAccessor httpContextAccessor, IElderRepo elderRepo, IEmailService emailService, IHobbyRepo hobbyRepo, IPsychomotorHealthRepo psychomotorHealthRepo, IHealthDetailRepo healthDetailRepo, IPsychomotorRepo psychomotorRepo, IServiceRepo serviceRepo, IPackageRepo packageRepo)
        {
            _context = context;
            _accountRepository = accountRepository;
            _contractRepository = contractRepository;
            _customerRepository = customerRepository;
            _carerRepository = carerRepository;
            _notificationService = notificationService;
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
            _elderRepo = elderRepo;
            _emailService = emailService;
            _hobbyRepo = hobbyRepo;
            _psychomotorHealthRepo = psychomotorHealthRepo;
            _healthDetailRepo = healthDetailRepo;
            _psychomotorRepo = psychomotorRepo;
            _serviceRepo = serviceRepo;
            _packageRepo = packageRepo;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public ICustomerRepository CustomerRepository => _customerRepository;
        public ICarerRepository CarerRepository => _carerRepository;
        public ITransactionRepo TransactionRepo => _transactionRepository;
 
        public Interfaces.INotificationService NotificationService => _notificationService;

        public IElderRepo ElderRepo => _elderRepo;

        public Interfaces.IEmailService emailService => _emailService;

        public IHobbyRepo HobbyRepo => _hobbyRepo;

        public IPsychomotorHealthRepo PsychomotorHealthRepo => _psychomotorHealthRepo;

        public IHealthDetailRepo HealthDetailRepo => _healthDetailRepo;
        public IContractRepository ContractRepository => _contractRepository;

        public IPsychomotorRepo PsychomotorRepo => _psychomotorRepo;

        public IServiceRepo ServiceRepo => _serviceRepo;

        public IPackageRepo PackageRepo => _packageRepo;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
