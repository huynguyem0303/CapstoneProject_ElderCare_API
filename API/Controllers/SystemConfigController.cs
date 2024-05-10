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
    [Authorize(Roles = "Admin")]
    public class SystemConfigsController : ODataController
    {
        private readonly ISystemConfigService _systemConfigService;

        public SystemConfigsController(ISystemConfigService systemConfigService)
        {
            _systemConfigService = systemConfigService;
        }

        // GET: api/SystemConfigs
        [HttpGet]
        [EnableQuery]
        public IActionResult GetSystemConfigs()
        {
            var list = _systemConfigService.GetAll();
            
            return Ok(list);
        }

        // GET: api/SystemConfigs/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult<SystemConfig>> GetSystemConfig(int id)
        {
            var systemConfig = await _systemConfigService.FindAsync(x => x.SystemConfigId == id);
            return SingleResult.Create(systemConfig.AsQueryable());
        }

        // PUT: api/SystemConfigs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutSystemConfig(int id, SystemConfig systemConfig)
        {
            if (id != systemConfig.SystemConfigId)
            {
                return BadRequest();
            }

            try
            {
                await _systemConfigService.UpdateSystemConfig(systemConfig);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _systemConfigService.SystemConfigExists(id))
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

        // POST: api/SystemConfigs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<SystemConfig>> PostSystemConfig(AddSystemConfigDto model)
        {
            SystemConfig systemConfig;
            try
            {
                systemConfig = await _systemConfigService.AddSystemConfigAsync(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetSystemConfig", new { id = systemConfig.SystemConfigId }, systemConfig);
        }

        // DELETE: api/SystemConfigs/5
        [HttpDelete("{id}")]
        [EnableQuery]
        public async Task<IActionResult> DeleteSystemConfig(int id)
        {
            if (!await _systemConfigService.SystemConfigExists(id))
            {
                return NotFound();
            }
            await _systemConfigService.DeleteSystemConfig(id);
            return NoContent();
        }
    }
}
