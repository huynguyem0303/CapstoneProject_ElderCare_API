using CorePush.Apple;
using CorePush.Google;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using ElderCare_Repository.Mappers;
using ElderCare_Repository.Repos;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElderCare_Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICarerRepository, CarerRepository>();
            services.AddScoped<ITransactionRepo, TransactionRepo>();
            services.AddScoped<IElderRepo, ElderRepo>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ISignupService, SignupService>();
            services.AddScoped<IElderService, ElderService>();
            services.AddScoped<IContractRepository, ContractRepo>();
            services.AddScoped<IContractService, Services.ContractService>();
            services.AddScoped<ICarerService, Services.CarerService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IHobbyRepo, HobbyRepo>();
            services.AddScoped<IPsychomotorHealthRepo, PsychomotorHealthRepo>();
            services.AddScoped<IHealthDetailRepo, HealthDetailRepo>();
            services.AddScoped<IPsychomotorRepo, PsychomotorRepo>();
            services.AddScoped<IPsychomotorService, PsychomotorService>();
            services.AddScoped<IServiceRepo, ServiceRepo>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IPackageRepo, PackageRepo>();
            services.AddScoped<IPackageService, PackagesService>();
            services.AddScoped<ICertificationRepo, CertificationRepo>();
            services.AddScoped<ICertificationService, CertificationService>();
            services.AddScoped<IReportRepo, ReportRepo>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ISystemConfigRepo, SystemConfigRepo>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigurationProfile).Assembly);
            services.AddDbContext<ElderCareContext>(option => option.UseSqlServer(databaseConnection).EnableSensitiveDataLogging());

            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            return services;
        }
    }
}
