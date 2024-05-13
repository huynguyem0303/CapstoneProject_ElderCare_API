using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Service;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ElderCare_Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElderController  : ODataController
    {
        private readonly IElderService _elderService;

        public ElderController(IElderService elderService)
        {
            _elderService = elderService;
        }
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetElders()
        {
            var list = _elderService.GetAll();

            return Ok(list);
        }

        [HttpGet("Customer/{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> GetEldersByCusId(int id)
        {
            var list = await _elderService.FindAsync(e=>e.CustomerId==id);
            if (list.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(list.ToList());
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult> GetElder([FromRoute]int id)
        {
            var elder = await _elderService.FindAsync(e => e.ElderlyId == id); ;
            return SingleResult.Create(elder.AsQueryable());
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{elderId}")]
        [ApiExplorerSettings(IgnoreApi = true)] //old elder put method
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PutElder(int id, Elderly elder)
        {
            if (id != elder.ElderlyId)
            {
                return BadRequest();
            }

            //_unitOfWork.ElderRepo.Update(model);

            try
            {
                await _elderService.UpdateElderly(elder);
                //await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _elderService.ElderExists(id))
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
        
        /// <summary>
        /// This method update elder detail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutElderDetail(int id, UpdateElderDto model)
        {
            if (id != model.ElderlyId)
            {
                return BadRequest();
            }

            try
            {
                await _elderService.UpdateElderlyDetail(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _elderService.ElderExists(id))
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

        /// <summary>
        /// This method update elder hobby
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{elderId}/Hobby/{id}")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutHobby(int elderId, int id, HobbyDto model)
        {
            if (elderId != model.ElderlyId || id != model.HobbyId)
            {
                return BadRequest();
            }
            if (!(await _elderService.HobbyExists(id) || await _elderService.ElderExists(elderId)))
            {
                return NotFound();
            }
            if (!await _elderService.ElderHobbyExist(elderId, id))
            {
                return NotFound();
            }
            try
            {
                await _elderService.UpdateElderlyHobby(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _elderService.ElderExists(elderId))
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

        /// <summary>
        /// This method update elder health detail
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{elderId}/HealthDetail/{id}")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutHealthDetail(int elderId, int id, UpdateHealthDetailDto model)
        {
            if (elderId != model.ElderlyId || id != model.HealthDetailId)
            {
                return BadRequest();
            }
            if (!await _elderService.ElderHealthDetailExist(elderId, id))
            {
                return NotFound();
            }
            try
            {
                await _elderService.UpdateElderlyHealthDetail(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _elderService.ElderExists(elderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// This method update elder psychomotor health detail
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="healthDetailId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{elderId}/HealthDetail/{healthDetailId}/PsychomotorHealth")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutPsychomotorHealth(int elderId, int healthDetailId, PsychomotorHealthDto model)
        {
            if (healthDetailId != model.HealthDetailId)
            {
                return BadRequest();
            }

            if (!await _elderService.ElderlyPsychomotorHealtExists(model.HealthDetailId, model.PsychomotorHealthId))
            {
                return NotFound();
            }
            if (!await _elderService.ElderHealthDetailExist(elderId, healthDetailId))
            {
                return NotFound();
            }

            try
            {
                await _elderService.UpdateElderlyPsychomotorHealth(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// This method add elder psychomotor health detail
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="healthDetailId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{elderId}/HealthDetail/{healthDetailId}/PsychomotorHealth")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PostPsychomotorHealth(int elderId , int healthDetailId, PsychomotorHealthDto model)
        {
            if (healthDetailId != model.HealthDetailId)
            {
                return BadRequest();
            }
            if (!await _elderService.ElderHealthDetailExist(elderId, healthDetailId))
            {
                return NotFound();
            }
            if (await _elderService.ElderlyPsychomotorHealtExists(model.HealthDetailId, model.PsychomotorHealthId))
            {
                return BadRequest("Can not insert duplicate object with the same ids in the db");
            }
            try
            {
                await _elderService.AddElderlyPsychomotorHealth(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        /// <summary>
        /// This method add elder health detail
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{elderId}/HealthDetail")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PostElderHealthDetail(int elderId, AddHealthDetailDto model)
        {
            if (elderId != model.ElderlyId)
            {
                return BadRequest();
            }
            HealthDetailDto healthDetail;
            try
            {
                healthDetail = await _elderService.AddElderlyHealthDetail(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _elderService.ElderExists(elderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElder", new { elderId }, healthDetail);
        }

        /// <summary>
        /// This method add elder hobby
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{elderId}/Hobby")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PostElderHobby(int elderId, AddElderHobbyDto model)
        {
            if (elderId != model.ElderlyId)
            {
                return BadRequest();
            }
            HobbyDto hobby;
            try
            {
                hobby = await _elderService.AddElderlyHobby(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _elderService.ElderExists(elderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElder", new { elderId }, hobby);
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Account>> PostElder(AddElderDto model)
        {
            ElderViewDto elder;
            try
            {
                elder = await _elderService.AddELderlyAsyncWithReturnDto(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("GetElder", new { id = elder.ElderlyId }, elder);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteElder(int id)
        {
            if(!await _elderService.ElderExists(id))
            {
                return NotFound();
            }
            await _elderService.DeleteElderly(id);

            return NoContent();
        }

        /// <summary>
        /// This method remove elder hobby
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{elderId}/Hobby/{id}")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteHobby(int elderId, int id)
        {
            if (!(await _elderService.HobbyExists(id) || await _elderService.ElderExists(elderId)))
            {
                return NotFound();
            }
            if (!await _elderService.ElderHobbyExist(elderId, id))
            {
                return NotFound();
            }
            await _elderService.DeleteHobby(id);

            return NoContent();
        }

        /// <summary>
        /// This method remove elder psychomotor health detail
        /// </summary>
        /// <param name="elderId"></param>
        /// <param name="healthDetailId"></param>
        /// <param name="psychomotorHealthId"></param>
        /// <returns></returns>
        [HttpDelete("{elderId}/HealthDetail/{healthDetailId}/PsychomotorHealth/{psychomotorHealthId}")]
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemovePsychomotorHealth(int elderId, int healthDetailId, int psychomotorHealthId)
        {
            if (!await _elderService.ElderlyPsychomotorHealtExists(healthDetailId, psychomotorHealthId))
            {
                return NotFound();
            }
            if (!await _elderService.ElderHealthDetailExist(elderId, healthDetailId))
            {
                return NotFound();
            }
            try
            {
                await _elderService.RemoveElderlyPsychomotorHealth(healthDetailId, psychomotorHealthId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}

