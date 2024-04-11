using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface ITimetableService
    {
        Task<Timetable> CreateTrackingTimetable(AddTimetableDto model);
    }
}
