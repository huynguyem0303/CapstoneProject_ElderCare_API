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

        public async Task UpdateTrackingByCarer(CarerUpdateTrackingDto model)
        {
            var tracking = await _unitOfWork.TrackingRepo.GetByIdAsync(model.TrackingId) ?? throw new DbUpdateException("Incorrect timetableId");
            if (tracking.TimetableId != model.TimetableId)
            {
                throw new DbUpdateException("Incorrect timetableId");
            }
            if (tracking.Status != (int?)TrackingStatus.Approved)
            {
                tracking.Status = (int?)TrackingStatus.Reported;
                tracking.ReportDate = DateTime.Now;
            }
            else
            {
                throw new DbUpdateException("This was already approved by the customer!");
            }
            _mapper.Map(model, tracking);
            _unitOfWork.TrackingRepo.Update(tracking);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task ApproveTracking(CustomerApproveTrackingDto model)
        {
            var tracking = await _unitOfWork.TrackingRepo.GetByIdAsync(model.TrackingId) ?? throw new DbUpdateException("Incorrect trackingId");
            if (tracking.TimetableId != model.TimetableId)
            {
                throw new DbUpdateException("Incorrect timetableId");
            }
            tracking.CusApprove = model.Status == (int?)TrackingStatus.Approved;
            _mapper.Map(model, tracking);
            _unitOfWork.TrackingRepo.Update(tracking);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Tracking> AddTrackingToTimetable(AddTrackingDto model)
        {
            var tracking = _mapper.Map<Tracking>(model);
            await _unitOfWork.TrackingRepo.AddAsync(tracking);
            await _unitOfWork.SaveChangeAsync();
            return tracking;
        }

        public async Task<IEnumerable<Tracking>> FindTrackingAsync(Expression<Func<Tracking, bool>> expression, params Expression<Func<Tracking, object>>[] includes)
        {
            return await _unitOfWork.TrackingRepo.FindAsync(expression, includes);
        }

        public async Task DeleteTracking(string trackingId)
        {
            var tracking = await _unitOfWork.TrackingRepo.GetByIdAsync(Guid.Parse(trackingId));
            _unitOfWork.TrackingRepo.Delete(tracking!);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> TrackingExisted(string trackingId)
        {
            return await _unitOfWork.TrackingRepo.GetByIdAsync(Guid.Parse(trackingId)) != null;
        }
    }
}
