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
    public class CertificationsController : ODataController
    {
        private readonly ICertificationService _certificationService;

        public CertificationsController(ICertificationService certificationService)
        {
            _certificationService = certificationService;
        }

        // GET: api/Certifications
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetCertifications()
        {
            var list = _certificationService.GetAll();
            
            return Ok(list);
        }

        // GET: api/Certifications/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Certification>> GetCertification(int id)
        {
            var certification = await _certificationService.FindAsync(x => x.CertId == id);
            return SingleResult.Create(certification.AsQueryable());
        }

        // PUT: api/Certifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> PutCertification(int id, UpdateCertificationTypeDto certification)
        {
            if (id != certification.CertId)
            {
                return BadRequest();
            }

            try
            {
                await _certificationService.UpdateCertification(certification);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _certificationService.CertificationExists(id))
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

        // POST: api/Certifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<ActionResult<Certification>> PostCertification(AddCertificationTypeDto model)
        {
            Certification certification;
            try
            {
                certification = await _certificationService.AddCertificationAsync(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetCertification", new { id = certification.CertId }, certification);
        }

        // DELETE: api/Certifications/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteCertification(int id)
        {
            if (await _certificationService.CertificationExists(id))
            {
                return NotFound();
            }
            await _certificationService.DeleteCertification(id);
            return NoContent();
        }
    }
}
