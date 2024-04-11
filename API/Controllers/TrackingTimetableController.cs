using DocumentFormat.OpenXml.Office2010.Excel;
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
        [Authorize]
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
            return CreatedAtAction("GetTrackingTimetable", new { id = timetable.TimetableId }, timetable);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult> GetTrackingTimetable(int id)
        {
            var timetable = await _timetableService.FindTimetableAsync(e => e.TimetableId == id, e => e.Trackings);
            return SingleResult.Create(timetable.AsQueryable());
        }

        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize]
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

        /// <summary>
        /// This method is for carer to report/update tracking in the timetable
        /// </summary>
        /// <param name="id">Timetable id</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}/Trackings/CarerReport")]
        [EnableQuery]
        //[Authorize("Carer")]
        public async Task<IActionResult> UpdateTrackingByCarer(int id, CarerUpdateTrackingDto model)
        {
            if (!await _timetableService.TimetableExist(id))
            {
                return NotFound();
            }

            if (id != model.TimetableId)
            {
                return BadRequest();
            }

            try
            {
                await _timetableService.UpdateTrackingByCarer(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// This method is for customer to approve tracking report in the timetable; 
        /// status: 2-Approved, 3-Unapproved
        /// </summary>
        /// <param name="id">Timetable id</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("{id}/Trackings/CustomerApprove")]
        [EnableQuery]
        //[Authorize("Customer")]
        public async Task<IActionResult> ApproveTrackingByCustomer(int id, CustomerApproveTrackingDto model)
        {
            if (!await _timetableService.TimetableExist(id))
            {
                return NotFound();
            }

            if (id != model.TimetableId)
            {
                return BadRequest();
            }

            try
            {
                await _timetableService.ApproveTracking(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// This method add new tracking to the timetable
        /// </summary>
        /// <param name="id">Timetable Id</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{id}/Trackings")]
        [EnableQuery]
        //[Authorize]
        public async Task<IActionResult> AddTrackingToTimetable(int id, AddTrackingDto model)
        {
            if (!await _timetableService.TimetableExist(id))
            {
                return NotFound();
            }

            if (id != model.TimetableId)
            {
                return BadRequest();
            }
            Tracking tracking;
            try
            {
                tracking = await _timetableService.AddTrackingToTimetable(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }
            return CreatedAtAction("GetTrackingTimetable", new { id = tracking.TimetableId }, tracking);
        }
    }
}
