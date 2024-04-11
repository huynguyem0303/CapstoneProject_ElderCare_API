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
        Task<Timetable> CreateTrackingTimetable(AddTimetableDto model);
        Task DeleteTimetable(int id);
        Task<IEnumerable<Timetable>> FindTimetableAsync(Expression<Func<Timetable, bool>> expression, params Expression<Func<Timetable, object>>[] includes);
        Task<bool> TimetableExist(int id);
        Task UpdateTimetable(UpdateTimetableDto model);
    }
}
