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
        private readonly IContractRepository _contractRepo;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarerRepository _carerRepository;
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
        private readonly ICertificationRepo _certificationRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IReportRepo _reportRepo;
        private readonly ISystemConfigRepo _systemConfigRepo;
        private readonly IFeedbackRepo _feedbackRepo;
        private readonly INotificationRepo _notificationRepo;

        public UnitOfWork(ElderCareContext context, IAccountRepository accountRepository, IContractRepository contractRepo, ICustomerRepository customerRepository, ICarerRepository carerRepository, ITransactionRepo transactionRepository, IHttpContextAccessor httpContextAccessor, IElderRepo elderRepo, IEmailService emailService, IHobbyRepo hobbyRepo, IPsychomotorHealthRepo psychomotorHealthRepo, IHealthDetailRepo healthDetailRepo, IPsychomotorRepo psychomotorRepo, IServiceRepo serviceRepo, IPackageRepo packageRepo, ICertificationRepo certificationRepo, ICategoryRepo categoryRepo, IReportRepo reportRepo, ISystemConfigRepo systemConfigRepo, IFeedbackRepo feedbackRepo, INotificationRepo notificationRepo)
        {
            _context = context;
            _accountRepository = accountRepository;
            _contractRepo = contractRepo;
            _customerRepository = customerRepository;
            _carerRepository = carerRepository;
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
            _certificationRepo = certificationRepo;
            _categoryRepo = categoryRepo;
            _reportRepo = reportRepo;
            _systemConfigRepo = systemConfigRepo;
            _feedbackRepo = feedbackRepo;
            _notificationRepo = notificationRepo;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public ICustomerRepository CustomerRepository => _customerRepository;

        public ICarerRepository CarerRepository => _carerRepository;

        public ITransactionRepo TransactionRepo => _transactionRepository;

        public IElderRepo ElderRepo => _elderRepo;

        public Interfaces.IEmailService emailService => _emailService;

        public IHobbyRepo HobbyRepo => _hobbyRepo;

        public IPsychomotorHealthRepo PsychomotorHealthRepo => _psychomotorHealthRepo;

        public IHealthDetailRepo HealthDetailRepo => _healthDetailRepo;

        public IContractRepository ContractRepo => _contractRepo;
        public ICategoryRepo CategoryRepo => _categoryRepo;

        public IServiceRepo ServiceRepo => _serviceRepo;

        public IPackageRepo PackageRepo => _packageRepo;

        public IPsychomotorRepo PsychomotorRepo => _psychomotorRepo;

        public ICertificationRepo CertificationRepo => _certificationRepo;

        public IReportRepo ReportRepo => _reportRepo;

        public ISystemConfigRepo SystemConfigRepo => _systemConfigRepo;

        public IFeedbackRepo FeedbackRepo => _feedbackRepo;

        public INotificationRepo NotificationRepo => _notificationRepo;

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
