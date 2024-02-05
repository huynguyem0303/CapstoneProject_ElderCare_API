using AutoMapper;
using CorePush.Apple;
using CorePush.Google;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using ElderCare_Repository.Mappers;
using ElderCare_Repository.Repos;
using ElderCare_Repository.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository
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
            services.AddAutoMapper(typeof(MapperConfigurationProfile).Assembly);
            services.AddDbContext<ElderCareContext>(option => option.UseSqlServer(databaseConnection).EnableSensitiveDataLogging());

            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            return services;
        }
    }
}
