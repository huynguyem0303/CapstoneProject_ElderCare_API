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
        public IElderRepo ElderRepo { get; }
        public IEmailService emailService { get; }


        public INotificationService NotificationService { get; }
    }
}

