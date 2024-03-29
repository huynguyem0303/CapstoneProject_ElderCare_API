﻿using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using System.Linq.Expressions;

namespace ElderCare_Service.Services
{
    public class AccountService : IAccountService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Account> AddAccountAsync(SignInDto model)
        {
            var account = _mapper.Map<Account>(model);
            account.Status = (int)AccountStatus.Active;
            account.RoleId = (int)AccountRole.None; 
            await _unitOfWork.AccountRepository.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            return account;
        }

        public async Task DeleteAccount(int id)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (account != null)
            {
                _unitOfWork.AccountRepository.Delete(account);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<Account> GetAll()
        {
            return _unitOfWork.AccountRepository.GetAll();
        }

        public async Task<IEnumerable<Account>> FindAsync(Expression<Func<Account, bool>> expression, params Expression<Func<Account, object>>[] includes)
        {
            return await _unitOfWork.AccountRepository.FindAsync(expression,includes);
        }

        public async Task UpdateAccount(Account account)
        {
            _unitOfWork.AccountRepository.Update(account);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Account?> GetById(int id)
        {
            return await _unitOfWork.AccountRepository.GetByIdAsync(id);
        }
        public async Task<bool> AccountExists(int id)
        {
            return await _unitOfWork.AccountRepository.GetByIdAsync(id) != null;
        }
    }
}
