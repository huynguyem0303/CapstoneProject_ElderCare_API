using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingTimetableController : ODataController
    {
        private readonly ITimetableService _timetableService;

        public TrackingTimetableController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        [HttpPost]
        [EnableQuery]
        public async Task<IActionResult> CreateTrackingTimetable(AddTimetableDto model)
        {
            Timetable timetable; 
            try
            {
                timetable = await _timetableService.CreateTrackingTimetable(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }
            return Ok(timetable);
        }
    }
}
