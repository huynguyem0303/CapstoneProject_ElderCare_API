using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
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

        public async Task<List<CarerTransactionDto>> GetCarerTransactionHistoryAsync(int carerId)
        {
            var transactionList = await _unitOfWork.CarerRepository.GetCarerTransaction(carerId);
            var carerTransactions = _mapper.Map<List<CarerTransactionDto>>(transactionList);
            foreach (var transaction in carerTransactions)
            {
                var carerCus = await _unitOfWork.CarerRepository.GetCarerCustomerFromIdAsync(transactionList[carerTransactions.IndexOf(transaction)].CarercusId);
                if (carerCus != null)
                {
                    (transaction.CarerId, transaction.CustomerId) = (carerCus.CarerId, carerCus.CustomerId);
                }
            }
            return carerTransactions;
        }

        public async Task<List<Carer>?> SearchCarer(SearchCarerDto dto)
        {
            return await _unitOfWork.CarerRepository.searchCarer(dto);
        }

        public async Task UpdateCarer(Carer carer)
        {
            _unitOfWork.CarerRepository.Update(carer);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
