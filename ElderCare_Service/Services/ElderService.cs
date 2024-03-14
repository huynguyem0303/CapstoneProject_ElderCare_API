﻿using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElderCare_Service.Services
{
    public class ElderService : IElderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ElderService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Elderly> AddELderlyAsync(AddElderDto model)
        {
            var elder = _mapper.Map<Elderly>(model);
            var id = _unitOfWork.ElderRepo.GetAll().OrderByDescending(i => i.ElderlyId).FirstOrDefault().ElderlyId;
            elder.ElderlyId = id + 1;
            await _unitOfWork.ElderRepo.AddAsync(elder);
            await _unitOfWork.SaveChangeAsync();
            return elder;
        }
        public async Task<ElderViewDto> AddELderlyAsyncWithReturnDto(AddElderDto model)
        {
            var elder = _mapper.Map<Elderly>(model);
            var id = _unitOfWork.ElderRepo.GetAll().OrderByDescending(i => i.ElderlyId).FirstOrDefault().ElderlyId;
            elder.ElderlyId = id + 1;
            await _unitOfWork.ElderRepo.AddAsync(elder);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<ElderViewDto>(elder);
        }

        public async Task DeleteElderly(int id)
        {
            var elder = await _unitOfWork.ElderRepo.GetByIdAsync(id);
            if (elder != null)
            {
                _unitOfWork.ElderRepo.Delete(elder);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> ElderExists(int id)
        {
            return await _unitOfWork.ElderRepo.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<Elderly>> FindAsync(Expression<Func<Elderly, bool>> expression, params Expression<Func<Elderly, object>>[] includes)
        {
            return await _unitOfWork.ElderRepo.FindAsync(expression, includes);
        }

        public IEnumerable<Elderly> GetAll()
        {
            return _unitOfWork.ElderRepo.GetAll();
        }

        public async Task<Elderly?> GetById(int id)
        {
            return await _unitOfWork.ElderRepo.GetByIdAsync(id);
        }

        public async Task UpdateElderly(Elderly elderly)
        {
            _unitOfWork.ElderRepo.Update(elderly);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateElderlyDetail(UpdateElderDto model)
        {
            var elderly = (await _unitOfWork.ElderRepo.FindAsync(e => e.ElderlyId == model.ElderlyId, p => p.Livingcondition)).First() ?? throw new DbUpdateConcurrencyException();
            _mapper.Map(model, elderly);
            _unitOfWork.ElderRepo.Update(elderly);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateElderlyHobby(HobbyDto model)
        {
            var hobby = await _unitOfWork.HobbyRepo.GetByIdAsync(model.HobbyId);
            _mapper.Map(model, hobby);
            _unitOfWork.HobbyRepo.Update(hobby);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<HobbyDto> AddElderlyHobby(AddElderHobbyDto model)
        {
            var hobby = _mapper.Map<Hobby>(model);
            hobby.HobbyId = _unitOfWork.HobbyRepo.GetAll().OrderBy(e => e.HobbyId).Select(e => e.HobbyId).Last() + 1;
            await _unitOfWork.HobbyRepo.AddAsync(hobby);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<HobbyDto>(hobby);
        }

        public async Task UpdateElderlyHealthDetail(UpdateHealthDetailDto model)
        {
            var elderly = (await _unitOfWork.ElderRepo.FindAsync(e => e.ElderlyId == model.ElderlyId, p => p.HealthDetail)).First() ?? throw new DbUpdateConcurrencyException();
            if (elderly.HealthDetailId != model.HealthDetailId)
            {
                throw new Exception("HealthDetailId doesn't match with elderly's HealthDetailId");
            }
            _mapper.Map(model, elderly.HealthDetail);
            _unitOfWork.HealthDetailRepo.Update(elderly.HealthDetail);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<HealthDetailDto> AddElderlyHealthDetail(AddHealthDetailDto model)
        {
            var healtDetail = _mapper.Map<HealthDetail>(model);
            healtDetail.HealthDetailId = _unitOfWork.HealthDetailRepo.GetAll().OrderBy(e => e.HealthDetailId).Select(e => e.HealthDetailId).Last() + 1;
            var psychomotorHealths = _mapper.Map<List<PsychomotorHealth>>(model.PsychomotorHealthDetails);
            await _unitOfWork.HealthDetailRepo.AddHealthDetail(model.ElderlyId, healtDetail);
            //foreach (var item in psychomotorHealths)
            //{
            //    item.HealthDetailId = healtDetail.HealthDetailId;
            //    await _unitOfWork.PsychomotorHealthRepo.AddAsync(item);
            //}
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<HealthDetailDto>(healtDetail);
        }
    }
}
