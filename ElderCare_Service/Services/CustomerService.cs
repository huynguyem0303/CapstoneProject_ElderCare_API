using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CustomerExists(int id)
        {
            return await _unitOfWork.CustomerRepository.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> expression, params Expression<Func<Customer, object>>[] includes)
        {
            return await _unitOfWork.CustomerRepository.FindAsync(expression, includes);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _unitOfWork.CustomerRepository.GetAll();
        }

        public IEnumerable<Carer> GetCarersByCustomerId(int customerId)
        {
            return _unitOfWork.CarerRepository.GetCarersByCustomerId(customerId);
        }

        public IEnumerable<Notification> GetNotificationsByCustomerId(int customerId)
        {
            return _unitOfWork.NotificationRepo.GetAllByCustomerId(customerId);
        }

        public async Task UpdateCustomer(UpdateCustomerDto model)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(model.CustomerId) ?? throw new DbUpdateConcurrencyException();
            var acc = (await _unitOfWork.AccountRepository.FindAsync(e => e.CustomerId == model.CustomerId)).SingleOrDefault() ?? throw new DbUpdateConcurrencyException();
            _mapper.Map(model, customer);
            _mapper.Map(customer, acc);
            _unitOfWork.CustomerRepository.Update(customer);
            _unitOfWork.AccountRepository.Update(acc);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
