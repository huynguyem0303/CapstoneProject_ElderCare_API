using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
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

        public async Task<Contract> AddContract(AddContractDto dto)
        {
            try
            {
                var entity = _mapper.Map<Contract>(dto);
                entity.ContractId = _unitOfWork.ContractRepo.GetAll().OrderByDescending(x => x.ContractId).FirstOrDefault().ContractId + 1;
                entity.Status = (int)ContractStatus.Pending;
                entity.CreatedDate = DateTime.Now;
                if (dto.PackageName.IsNullOrEmpty())
                {
                    _unitOfWork.ContractRepo.AddContractServiceAsync(dto.service, entity.ContractId);
                    entity.Packageprice = _unitOfWork.ContractRepo.GetPackagePrice().Result;
                    entity.ContractType = (int)ContractType.ServiceContract;
                  ;
                }
                else if (dto.service.IsNullOrEmpty())
                {
                    entity.Package = _unitOfWork.ContractRepo.GetPackageAsync(dto.PackageName).Result;
                    entity.PackageId = _unitOfWork.ContractRepo.GetPackageAsync(dto.PackageName).Result.PackageId;
                    entity.Packageprice = _unitOfWork.ContractRepo.GetPackagePrice().Result;
                    entity.ContractType = (int)ContractType.PackageContract;
                }
                await _unitOfWork.ContractRepo.AddContractVersionAsync(dto.startDate, dto.endDate, entity.ContractId);
                await _unitOfWork.ContractRepo.AddAsync(entity);
                await _unitOfWork.SaveChangeAsync();
                return entity;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<List<Contract>> GetByCarerId(int id)
        {
            return _unitOfWork.ContractRepo.GetByCarer(id).Result.ToList();
        }
        public async Task<IEnumerable<Contract>> FindAsync(Expression<Func<Contract, bool>> expression, params Expression<Func<Contract, object>>[] includes)
        {
            return await _unitOfWork.ContractRepo.FindAsync(expression, includes);
        }


        public async Task<Contract?> ApproveContract(int contractid, int status)
        {
            var contract = await _unitOfWork.ContractRepo.GetByIdAsync(contractid) ?? throw new Exception("contract not found");
            contract.Status = status;
            _unitOfWork.ContractRepo.Update(contract);
            await _unitOfWork.SaveChangeAsync();
            return contract;
        }

        public async Task<bool> ContractExists(int id)
        {
            return await _unitOfWork.ContractRepo.GetByIdAsync(id) != null;
        }


        public async Task<(Contract, List<Timetable>)> AddContract2(AddContractWithTrackingsDto dto)
        {
            var contract = await AddContract(dto);

            var addTimetableDtos = _mapper.Map<List<AddTimetableDto>>(dto.TrackingTimetables);
            var trackingTimeables = new List<Timetable>();
            foreach (var item in addTimetableDtos)
            {
                item.ContractId = contract.ContractId;
                item.CarerId = contract.CarerId;
                if (await _unitOfWork.ContractRepo.IsContractExpired((int)item.ContractId!))
                {
                    throw new Exception("This contract has all ready expired");
                }
                var timetable = _mapper.Map<Timetable>(item);
                timetable.TimetableId = _unitOfWork.TimetableRepo.GetAll().OrderBy(e => e.TimetableId).Last().TimetableId + 1;
                await _unitOfWork.TimetableRepo.AddAsync(timetable);
                trackingTimeables.Add(timetable);
            }
            await _unitOfWork.SaveChangeAsync();
            return (contract, trackingTimeables);
        }

        public async Task ExpriedContract()
        {
            await _unitOfWork.ContractRepo.ExpriedContract();

            await _unitOfWork.SaveChangeAsync();


        }

        public async Task<List<Contract>> ExpriedContractToday()
        {
            return await _unitOfWork.ContractRepo.ExpriedContractToday();
        }

        public async Task<List<Contract>> ExpriedContractInNext5Day()
        {
            return await _unitOfWork.ContractRepo.ExpriedContractInNext5Day();
        }
    }
}

