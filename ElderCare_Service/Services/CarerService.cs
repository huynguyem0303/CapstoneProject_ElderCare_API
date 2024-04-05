using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class CarerService : ICarerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

       

        //public async Task<carer> AddCarerAsync(carer model)
        //{
        //    await _unitOfWork.CarerRepository.AddAsync(model);
        //    await _unitOfWork.SaveChangeAsync();
        //    return model;
        //}

        public async Task<bool> CarerExists(int id)
        {
            return await _unitOfWork.CarerRepository.GetByIdAsync(id) != null;
        }

        public async Task<Account?> ApproveCarer(int carerId, int status)
        {
            var carer = await _unitOfWork.CarerRepository.GetByIdAsync(carerId) ?? throw new Exception("carer not found");
            carer.Status = status;
            var account = (await _unitOfWork.AccountRepository.FindAsync(e => e.CarerId == carerId)).FirstOrDefault();
            if (status == (int)CarerStatus.Approved && account == null)
            {
                account = _mapper.Map<Account>(carer);
                account.RoleId = (int)AccountRole.Carer;
                account.Status = (int)AccountStatus.Active;
                account.CreatedDate = DateTime.Now;
                string randomString = Guid.NewGuid().ToString("N").Substring(0, 10);
                account.Password = randomString;
                await _unitOfWork.AccountRepository.AddAsync(account);
            }
            _unitOfWork.CarerRepository.Update(carer);
            await _unitOfWork.SaveChangeAsync();
            return account;
        }

        public async Task DeleteCarer(int id)
        {
            var carer = await _unitOfWork.CarerRepository.GetByIdAsync(id); 
            if (carer == null)
            {
                throw new Exception("carer Not Found");
            }
            carer.Status = (int?)AccountStatus.InActive; 
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<Carer>> FindAsync(Expression<Func<Carer, bool>> expression, params Expression<Func<Carer, object>>[] includes)
        {
            return await _unitOfWork.CarerRepository.FindAsync(expression, includes);
        }

        public IEnumerable<Carer> GetAll()
        {
            return _unitOfWork.CarerRepository.GetAll();
        }

        public async Task<Carer?> GetById(int id)
        {
            return await _unitOfWork.CarerRepository.GetByIdAsync(id);
        }

        public async Task<List<CarerViewDto>> GetByPending()
        {
            var carer= await _unitOfWork.CarerRepository.GetPendingCarerAsync();
            var carerview = _mapper.Map<List<CarerViewDto>>(carer);
            return carerview;
        }

        public async Task<List<TransactionHistoryDto>> GetTransactionHistoryByCarerIdAsync(int carerId)
        {
            var transactionList = await _unitOfWork.CarerRepository.GetCarerTransaction(carerId);
            if (transactionList.IsNullOrEmpty())
            {
                return null;
            }
            var carerTransactions = _mapper.Map<List<TransactionHistoryDto>>(transactionList);
            foreach (var transaction in carerTransactions)
            {
                var carerCus = await _unitOfWork.CarerRepository.GetCarerCustomerFromIdAsync(transactionList[carerTransactions.IndexOf(transaction)].CarercusId);
                if (carerCus != null)
                {
                    (transaction.CarerId, transaction.CustomerId) = (carerCus.CarerId, carerCus.CustomerId);
                    transaction.CarerName = (await _unitOfWork.CarerRepository.GetByIdAsync(carerCus.CarerId)).Name;
                    transaction.CustomerName = (await _unitOfWork.CustomerRepository.GetByIdAsync(carerCus.CustomerId)).CustomerName;
                }
            }
            return carerTransactions;
        }
        public async Task<List<TransactionHistoryDto>> GetTransactionHistoryByCustomerIdAsync(int customerId)
        {
            var transactionList = await _unitOfWork.CarerRepository.GetCustomerTransaction(customerId);
            if (transactionList.IsNullOrEmpty())
            {
                return null;
            }
            var carerTransactions = _mapper.Map<List<TransactionHistoryDto>>(transactionList);
            foreach (var transaction in carerTransactions)
            {
                var carerCus = await _unitOfWork.CarerRepository.GetCarerCustomerFromIdAsync(transactionList[carerTransactions.IndexOf(transaction)].CarercusId);
                if (carerCus != null)
                {
                    (transaction.CarerId, transaction.CustomerId) = (carerCus.CarerId, carerCus.CustomerId);
                    transaction.CarerName = (await _unitOfWork.CarerRepository.GetByIdAsync(carerCus.CarerId)).Name;
                    transaction.CustomerName = (await _unitOfWork.CustomerRepository.GetByIdAsync(carerCus.CustomerId)).CustomerName;
                }
            }
            return carerTransactions;
        }

        public async Task<List<CarerViewDto>?> SearchCarer(SearchCarerDto dto)
        {
            var carer = await _unitOfWork.CarerRepository.searchCarer(dto);
            var entity = _mapper.Map<List<CarerViewDto>>(carer);
            return entity;
        }

        public async Task UpdateCarer(Carer carer)
        {
            _unitOfWork.CarerRepository.Update(carer);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<List<ServiceDto>> GetServicesByCarerId(int id)
        {
            var services = await _unitOfWork.ServiceRepo.GetAllByCarerId(id);
            return _mapper.Map<List<ServiceDto>>(services);
        }

        public async Task<IEnumerable<Category>> FindCateAsync(Expression<Func<Category, bool>> expression, params Expression<Func<Category, object>>[] includes)
        {
            return await _unitOfWork.CategoryRepo.FindAsync(expression, includes);
        }

        public async Task<List<CarerServiceDto>> AddCarerServiceAsync(int carerId, string[] serviceName)
        {
            var result = await _unitOfWork.CarerRepository.AddCarerService(carerId, serviceName);
            var list = _mapper.Map<List<CarerServiceDto>>(result);
            foreach (var item in list)
            {
                item.ServiceName = (await _unitOfWork.ServiceRepo.GetByIdAsync(item.ServiceId!))!.Name;
            }
            await _unitOfWork.SaveChangeAsync();
            return list;
        }

        public async Task RemoveCarerService(int carerId, int serviceId)
        {
            await _unitOfWork.CarerRepository.RemoveCarerService(carerId, serviceId);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<List<FeedbackDto>> FindCarerFeedback(Expression<Func<Feedback, bool>> expression)
        {
            var feedbacks = await _unitOfWork.FeedbackRepo.FindAsync(expression, e => e.CarerService);
            return _mapper.Map<List<FeedbackDto>>(feedbacks);
        }

        public async Task<FeedbackDto> AddServiceFeedback(AddFeedbackDto model)
        {
            if ( (await _unitOfWork.CustomerRepository.GetByIdAsync(model.CustomerId)) == null)
            {
                throw new DbUpdateException("Your customerId doesn't exist");
            }
            var feedback = _mapper.Map<Feedback>(model);
            feedback.FeedbackId = _unitOfWork.FeedbackRepo.GetAll().OrderBy(e => e.FeedbackId).Last().FeedbackId + 1;
            feedback.CarerServiceId = (await _unitOfWork.CarerRepository.GetCarerService(model.CarerId, model.ServiceId)
                                       ?? throw new DbUpdateException("This carer doesn't have this service")
                ).CarerServiceId;
            feedback.CreatedDate = DateTime.Now;
            await _unitOfWork.FeedbackRepo.AddAsync(feedback);
            await _unitOfWork.SaveChangeAsync();
            var result = _mapper.Map<FeedbackDto>(feedback);
            result.CarerId = model.CarerId;
            result.ServiceId = model.ServiceId;
            return result;
        }

        public async Task UpdateCarerServiceFeedback(UpdateFeedbackDto model)
        {
            var feedback = await _unitOfWork.FeedbackRepo.GetByIdAsync(model.FeedbackId) ?? throw new DbUpdateException();
            _mapper.Map(model, feedback);
            _unitOfWork.FeedbackRepo.Update(feedback);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task RemoveCarerServiceFeedback(int feedbackId)
        {
            var feedback = await _unitOfWork.FeedbackRepo.GetByIdAsync(feedbackId) ?? throw new DbUpdateException();
            _unitOfWork.FeedbackRepo.Delete(feedback);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> FeedbackExist(int carerId, int serviceId, int feedbackId)
        {
            return !(await _unitOfWork.FeedbackRepo.FindAsync(e => e.CarerService.CarerId == carerId
            && e.CarerService.ServiceId == serviceId && e.FeedbackId == feedbackId)).IsNullOrEmpty();
        }
    }
}
