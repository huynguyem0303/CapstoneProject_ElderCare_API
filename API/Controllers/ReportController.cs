using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderCare_Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ElderCare_Repository.DTO;
using System.Data;
using ElderCare_Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ODataController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/Reports
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetReports()
        {
            var list = _reportService.GetAll();
            
            return Ok(list);
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Report>> GetReport(int id)
        {
            var report = await _reportService.FindAsync(x => x.ReportId == id);
            return SingleResult.Create(report.AsQueryable());
        }

        // PUT: api/Reports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PutReport(int id, UpdateReportDto report)
        {
            if (id != report.ReportId)
            {
                return BadRequest();
            }

            try
            {
                await _reportService.UpdateReport(report);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _reportService.ReportExists(id))
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

        // POST: api/Reports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        [Authorize]
        public async Task<ActionResult<Report>> PostReport(AddReportDto model)
        {
            Report report;
            try
            {
                report = await _reportService.AddReportAsync(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetReport", new { id = report.ReportId }, report);
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            if (!await _reportService.ReportExists(id))
            {
                return NotFound();
            }
            await _reportService.DeleteReport(id);
            return NoContent();
        }
    }
}
