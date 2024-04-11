﻿using AutoMapper;
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
    public class TimetableService : ITimetableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TimetableService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Timetable> CreateTrackingTimetable(AddTimetableDto model)
        {
            var timetable = _mapper.Map<Timetable>(model);
            timetable.TimetableId = _unitOfWork.TimetableRepo.GetAll().OrderBy(e => e.TimetableId).Last().TimetableId + 1;
            await _unitOfWork.TimetableRepo.AddAsync(timetable);
            await _unitOfWork.SaveChangeAsync();
            return timetable;
        }

        public async Task DeleteTimetable(int id)
        {
            var timetable = await _unitOfWork.TimetableRepo.GetByIdAsync(id);
            _unitOfWork.TimetableRepo.Delete(timetable!);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<Timetable>> FindTimetableAsync(Expression<Func<Timetable, bool>> expression, params Expression<Func<Timetable, object>>[] includes)
        {
            return await _unitOfWork.TimetableRepo.FindAsync(expression, includes);
        }

        public async Task<bool> TimetableExist(int id)
        {
            return await _unitOfWork.TimetableRepo.GetByIdAsync(id) != null;
        }

        public async Task UpdateTimetable(UpdateTimetableDto model)
        {
            _unitOfWork.TimetableRepo.Update(_mapper.Map<Timetable>(model));
            await _unitOfWork.SaveChangeAsync();
        }
    }
}