using ElderCare_Repository.Interfaces;
using ElderCare_Service.Interfaces;
namespace ElderCare_Service
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangeAsync();
        public IAccountRepository AccountRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public ICarerRepository CarerRepository { get; }
        public ITransactionRepo TransactionRepo { get; }
        public IContractRepository ContractRepo { get; }
        public ICategoryRepo CategoryRepo { get; }
        public IElderRepo ElderRepo { get; }
        public IHobbyRepo HobbyRepo { get; }
        public IPsychomotorHealthRepo PsychomotorHealthRepo { get; }
        public IHealthDetailRepo HealthDetailRepo { get; }
        public IEmailService emailService { get; }
        public IPsychomotorRepo PsychomotorRepo { get; }
        public IServiceRepo ServiceRepo { get; }
        public IPackageRepo PackageRepo { get; }
        public ICertificationRepo CertificationRepo { get; }
        public IReportRepo ReportRepo { get; }
        public ISystemConfigRepo SystemConfigRepo { get; }
        public IFeedbackRepo FeedbackRepo { get; }
        public INotificationRepo NotificationRepo { get; }
    }
}

