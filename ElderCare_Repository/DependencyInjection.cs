using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using ElderCare_Repository.Mappers;
using ElderCare_Repository.Repos;
using Microsoft.EntityFrameworkCore;
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

            services.AddAutoMapper(typeof(MapperConfigurationProfile).Assembly);
            services.AddDbContext<ElderCareContext>(option => option.UseSqlServer(databaseConnection).EnableSensitiveDataLogging());
            return services;
        }
    }
}
