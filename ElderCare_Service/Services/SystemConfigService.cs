using AutoMapper;
using ElderCare_Domain.Enums;
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
    public class SystemConfigService : ISystemConfigService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SystemConfigService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SystemConfig> AddSystemConfigAsync(AddSystemConfigDto model)
        {
            var systemConfig = _mapper.Map<SystemConfig>(model);
            systemConfig.SystemConfigId = _unitOfWork.SystemConfigRepo.GetAll().OrderBy(e => e.SystemConfigId).Last().SystemConfigId + 1;
            await _unitOfWork.SystemConfigRepo.AddAsync(systemConfig);
            await _unitOfWork.SaveChangeAsync();
            return systemConfig;
        }

        public async Task DeleteSystemConfig(int id)
        {
            var systemConfig = await _unitOfWork.SystemConfigRepo.GetByIdAsync(id);
            if (systemConfig != null)
            {
                _unitOfWork.SystemConfigRepo.Delete(systemConfig);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<SystemConfig> GetAll()
        {
            return _unitOfWork.SystemConfigRepo.GetAll();
        }

        public async Task<IEnumerable<SystemConfig>> FindAsync(Expression<Func<SystemConfig, bool>> expression, params Expression<Func<SystemConfig, object>>[] includes)
        {
            return await _unitOfWork.SystemConfigRepo.FindAsync(expression, includes);
        }

        public async Task UpdateSystemConfig(SystemConfig model)
        {
            _unitOfWork.SystemConfigRepo.Update(model);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<SystemConfig?> GetById(int id)
        {
            return await _unitOfWork.SystemConfigRepo.GetByIdAsync(id);
        }
        public async Task<bool> SystemConfigExists(int id)
        {
            return await _unitOfWork.SystemConfigRepo.GetByIdAsync(id) != null;
        }
    }
}
