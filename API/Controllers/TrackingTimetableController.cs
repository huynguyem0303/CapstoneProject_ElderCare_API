using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
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
            return CreatedAtAction("GetTrackingTimetable", new {id = timetable.TimetableId}, timetable);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult> GetTrackingTimetable(int id)
        {
            var timetable = await _timetableService.FindTimetableAsync(e => e.TimetableId == id, e => e.Trackings);
            return SingleResult.Create(timetable.AsQueryable());
        }

        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutTrackingTimetable(int id, UpdateTimetableDto model)
        {
            if (id != model.TimetableId)
            {
                return BadRequest();
            }

            try
            {
                await _timetableService.UpdateTimetable(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _timetableService.TimetableExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> RemoveTrackingTimetable(int id)
        {
            if (!await _timetableService.TimetableExist(id))
            {
                return NotFound();
            }
            await _timetableService.DeleteTimetable(id);
            return NoContent();
        }
    }
}
