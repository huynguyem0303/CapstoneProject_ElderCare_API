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
    public class ServicesController : ODataController
    {
        private readonly IServicesService _serviceService;

        public ServicesController(IServicesService serviceService)
        {
            _serviceService = serviceService;
        }

        // GET: api/Services
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetServices()
        {
            var list = _serviceService.GetAll();
            
            return Ok(list);
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Service>> GetService(int id)
        {
            var service = await _serviceService.FindAsync(x => x.ServiceId == id);
            return SingleResult.Create(service.AsQueryable());
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> PutService(int id, UpdateServiceDto service)
        {
            if (id != service.ServiceId)
            {
                return BadRequest();
            }

            try
            {
                await _serviceService.UpdateService(service);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _serviceService.ServiceExists(id))
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

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<ActionResult<Service>> PostService(AddServiceDto model)
        {
            Service service;
            try
            {
                service = await _serviceService.AddServiceAsync(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetService", new { id = service.ServiceId }, service);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (!await _serviceService.ServiceExists(id))
            {
                return NotFound();
            }
            await _serviceService.DeleteService(id);
            return NoContent();
        }
    }
}
