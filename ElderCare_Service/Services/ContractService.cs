using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using ElderCare_Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Contract> AddContract(int cusid, int carerid, int elderlyid, DateTime startDate, DateTime endDate, string package, string[] service)
        {
            var entity = new Contract();
            entity.ContractId = _unitOfWork.ContractRepository.GetAll().OrderByDescending(x => x.ContractId).FirstOrDefault().ContractId + 1;
            entity.CustomerId = cusid;
            entity.CarerId = carerid;
            entity.ElderlyId = elderlyid;
            entity.Status = (int)ContractStatus.Pending;
            entity.ContractType = 0;
            if (package.IsNullOrEmpty())
            {
                _unitOfWork.ContractRepository.AddContractServiceAsync(service, entity.ContractId);
                entity.Packageprice = _unitOfWork.ContractRepository.GetPackagePrice().Result;
                entity.PackageId = 0;
            }
            entity.PackageId = _unitOfWork.ContractRepository.GetPackageAsync(package).Result.PackageId;
            entity.Packageprice = _unitOfWork.ContractRepository.GetPackagePrice().Result;
            _unitOfWork.ContractRepository.AddContractVersionAsync(startDate, endDate, entity.ContractId);
            await _unitOfWork.ContractRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return entity;
        }

        public async Task<List<Contract>> GetByCarerId(int id)
        {
           return _unitOfWork.ContractRepository.GetByCarer(id).Result.ToList();
        }
        public async Task<IEnumerable<Contract>> FindAsync(Expression<Func<Contract, bool>> expression, params Expression<Func<Contract, object>>[] includes)
        {
            return await _unitOfWork.ContractRepository.FindAsync(expression, includes);
        }


        public async Task<Contract?> ApproveContract(int contractid, int status)
        {
            var contract = await _unitOfWork.ContractRepository.GetByIdAsync(contractid) ?? throw new Exception("contract not found");
            contract.Status = status;
            _unitOfWork.ContractRepository.Update(contract);
            await _unitOfWork.SaveChangeAsync();
            return contract;
        }
    }
}
