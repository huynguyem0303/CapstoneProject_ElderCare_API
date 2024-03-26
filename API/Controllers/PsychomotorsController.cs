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
    public class PsychomotorsController : ODataController
    {
        private readonly IPsychomotorService _psychomotorService;

        public PsychomotorsController(IPsychomotorService psychomotorService)
        {
            _psychomotorService = psychomotorService;
        }

        // GET: api/Psychomotors
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetPsychomotors()
        {
            var list = _psychomotorService.GetAll();
            
            return Ok(list);
        }

        // GET: api/Psychomotors/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Psychomotor>> GetPsychomotor(int id)
        {
            var psychomotor = await _psychomotorService.FindAsync(x => x.PsychomotorHealthId == id);
            return SingleResult.Create(psychomotor.AsQueryable());
        }

        // PUT: api/Psychomotors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> PutPsychomotor(int id, UpdatePsychomotorDto psychomotor)
        {
            if (id != psychomotor.PsychomotorHealthId)
            {
                return BadRequest();
            }

            try
            {
                await _psychomotorService.UpdatePsychomotor(psychomotor);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _psychomotorService.PsychomotorExists(id))
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

        // POST: api/Psychomotors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<ActionResult<Psychomotor>> PostPsychomotor(AddPsychomotorDto model)
        {
            Psychomotor psychomotor;
            try
            {
                psychomotor = await _psychomotorService.AddPsychomotorAsync(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetPsychomotor", new { id = psychomotor.PsychomotorHealthId }, psychomotor);
        }

        // DELETE: api/Psychomotors/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeletePsychomotor(int id)
        {
            if (await _psychomotorService.PsychomotorExists(id))
            {
                return NotFound();
            }
            await _psychomotorService.DeletePsychomotor(id);
            return NoContent();
        }
    }
}
