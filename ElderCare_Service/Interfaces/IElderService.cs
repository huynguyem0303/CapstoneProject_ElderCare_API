﻿using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface IElderService
    {
        IEnumerable<Elderly> GetAll();
        Task<Elderly?> GetById(int id);
        Task<IEnumerable<Elderly>> FindAsync(Expression<Func<Elderly, bool>> expression, params Expression<Func<Elderly, object>>[] includes);
        Task<Elderly> AddELderlyAsync(AddElderDto model);
        Task UpdateElderly(Elderly elderly);
        Task DeleteElderly(int id);
        Task<bool> ElderExists(int id);
        Task UpdateElderlyDetail(UpdateElderDto model);
        Task<ElderViewDto> AddELderlyAsyncWithReturnDto(AddElderDto model);
        Task UpdateElderlyHobby(HobbyDto model);
        Task<HobbyDto> AddElderlyHobby(AddElderHobbyDto model);
        Task UpdateElderlyHealthDetail(UpdateHealthDetailDto model);
        Task<HealthDetailDto> AddElderlyHealthDetail(AddHealthDetailDto model);
        Task UpdateElderlyPsychomotorHealth(PsychomotorHealthDto model);
        Task AddElderlyPsychomotorHealth(PsychomotorHealthDto model);
        Task<bool> ElderlyPsychomotorHealtExists(int HealthDetailId, int PsychomotorHealthId);
        Task<bool> HobbyExists(int id);
        Task DeleteHobby(int id);
        Task RemoveElderlyPsychomotorHealth(int healthDetailId, int psychomotorHealthId);
        Task<bool> ElderHobbyExist(int elderId, int hobbyId);
        Task<bool> ElderHealthDetailExist(int elderId, int healthDetailId);
    }
}
