﻿using AutoMapper;
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
    public class ServicesService : IServicesService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServicesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Service> AddServiceAsync(AddServiceDto model)
        {
            var service = _mapper.Map<Service>(model);
            service.ServiceId = _unitOfWork.ServiceRepo.GetAll().OrderBy(e => e.ServiceId).Last().ServiceId + 1;
            await _unitOfWork.ServiceRepo.AddAsync(service);
            await _unitOfWork.SaveChangeAsync();
            return service;
        }

        public async Task DeleteService(int id)
        {
            var service = await _unitOfWork.ServiceRepo.GetByIdAsync(id);
            if (service != null)
            {
                _unitOfWork.ServiceRepo.Delete(service);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<Service> GetAll()
        {
            return _unitOfWork.ServiceRepo.GetAll();
        }

        public async Task<IEnumerable<Service>> FindAsync(Expression<Func<Service, bool>> expression, params Expression<Func<Service, object>>[] includes)
        {
            return await _unitOfWork.ServiceRepo.FindAsync(expression, includes);
        }

        public async Task UpdateService(UpdateServiceDto model)
        {
            _unitOfWork.ServiceRepo.Update(_mapper.Map<Service>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Service?> GetById(int id)
        {
            return await _unitOfWork.ServiceRepo.GetByIdAsync(id);
        }
        public async Task<bool> ServiceExists(int id)
        {
            return await _unitOfWork.ServiceRepo.GetByIdAsync(id) != null;
        }

        public IEnumerable<Carer> GetCarerByServiceId(int serviceId)
        {
            return _unitOfWork.CarerRepository.GetCarerByServiceId(serviceId);
        }

        public async Task<TrackingOption> AddTrackingOption(AddTrackingOptionDto model)
        {
            var trackingOption = _mapper.Map<TrackingOption>(model);
            trackingOption.TrackingOptionId = (_unitOfWork.TrackingOptionRepo.GetAll()
                .OrderBy(e => e.TrackingOptionId)
                .LastOrDefault() ?? new TrackingOption())
                .TrackingOptionId + 1;
            await _unitOfWork.TrackingOptionRepo.AddAsync(trackingOption);
            await _unitOfWork.SaveChangeAsync();
            return trackingOption;
        }

        public async Task UpdateTrackingOption(UpdateTrackingOptionDto model)
        {
            _unitOfWork.TrackingOptionRepo.Update(_mapper.Map<TrackingOption>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteTrackingOption(int id)
        {
            var trackingOption = await _unitOfWork.TrackingOptionRepo.GetByIdAsync(id);
            if (trackingOption != null)
            {
                _unitOfWork.TrackingOptionRepo.Delete(trackingOption);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> TrackingOptionExists(int id)
        {
            return await _unitOfWork.TrackingOptionRepo.GetByIdAsync(id) != null;
        }

        public async Task<bool> ServiceNameExists(string name)
        {
            var check = await _unitOfWork.ServiceRepo.GetServiceByName(name);
            if (check != null)
            {
                return true;
            }
            return false;
        }
    }
}
