using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface ITimetableService
    {
        Task<Tracking> AddTrackingToTimetable(AddTrackingDto model);
        Task ApproveTracking(CustomerApproveTrackingDto model);
        Task<Timetable> CreateTrackingTimetable(AddTimetableDto model);
        Task DeleteTimetable(int id);
        Task DeleteTracking(string trackingId);
        Task<IEnumerable<Timetable>> FindTimetableAsync(Expression<Func<Timetable, bool>> expression, params Expression<Func<Timetable, object>>[] includes);
        Task<IEnumerable<Tracking>> FindTrackingAsync(Expression<Func<Tracking, bool>> expression, params Expression<Func<Tracking, object>>[] includes);
        Task<bool> TimetableExist(int id);
        Task<bool> TrackingExisted(string trackingId);
        Task UpdateTimetable(UpdateTimetableDto model);
        Task UpdateTrackingByCarer(CarerUpdateTrackingDto model);
    }
}
