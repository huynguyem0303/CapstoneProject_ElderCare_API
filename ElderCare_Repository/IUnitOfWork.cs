﻿using ElderCare_Repository.Interfaces;
using ElderCare_Repository.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository
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

