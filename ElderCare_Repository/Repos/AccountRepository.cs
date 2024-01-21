﻿using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class AccountRepository : GenericRepo<Account>, IAccountRepository
    {
        public AccountRepository(ElderCareContext context) : base(context)
        {
        }
        public async Task AddAsync(Account entity)
        {
            try
            {
                entity.AccountId = _dbSet.Last().AccountId + 1;
                entity.Status = true;
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
