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
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;
        private readonly IElderService _elderService;

        public ElderController(IElderService elderService)
        {
            _elderService = elderService;
        }

        //public ElderController(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}
        // GET: api/Accounts
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetElders()
        {
            //var list = _unitOfWork.ElderRepo.GetAll();
            var list = _elderService.GetAll();

            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult> GetElder([FromRoute]int id)
        {
            //var model = await _unitOfWork.ElderRepo.FindAsync(x => x.ElderlyId == id);
            var elder = await _elderService.FindAsync(e => e.ElderlyId == id); ;
            return SingleResult.Create(elder.AsQueryable());
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        
        [HttpPut("Update/{id}")]
        [EnableQuery]
        [Authorize]
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

        [HttpPut("Update/{id}/Hobby")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PutHobby(int id, HobbyDto model)
        {
            if (id != model.ElderlyId)
            {
                return BadRequest();
            }

            try
            {
                await _elderService.UpdateElderlyHobby(model);
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

        [HttpPut("Update/{id}/HealthDetail")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PutHealthDetail(int id, UpdateHealthDetailDto model)
        {
            if (id != model.ElderlyId)
            {
                return BadRequest();
            }

            try
            {
                await _elderService.UpdateElderlyHealthDetail(model);
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
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("Update/{id}/HealthDetail")]
        [EnableQuery]
        //[Authorize]
        public async Task<IActionResult> PostElderHealthDetail(int id, AddHealthDetailDto model)
        {
            if (id != model.ElderlyId)
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
                if (!await _elderService.ElderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElder", new { id }, healthDetail);
        }

        [HttpPost("Update/{id}/Hobby")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PostElderHobby(int id, AddElderHobbyDto model)
        {
            if (id != model.ElderlyId)
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
                if (!await _elderService.ElderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElder", new { id }, hobby);
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<Account>> PostElder(AddElderDto model)
        {
            //var model = _mapper.Map<Elderly>(model);
            //var id = _unitOfWork.ElderRepo.GetAll().OrderByDescending(i => i.ElderlyId).FirstOrDefault().ElderlyId;
            //model.ElderlyId = id+1;
            //await _unitOfWork.ElderRepo.AddAsync(model);
            //Elderly elder;
            ElderViewDto elder;
            try
            {
                //await _unitOfWork.SaveChangeAsync();
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
        [Authorize]
        public async Task<IActionResult> DeleteElder(int id)
        {
            //if ((_unitOfWork.ElderRepo.GetAll()).IsNullOrEmpty())
            //{
            //    return NotFound();
            //}
            //var model = await _unitOfWork.ElderRepo.GetByIdAsync(id);
            //if (model == null)
            //{
            //    return NotFound();
            //}

            //_unitOfWork.ElderRepo.Delete(model);
            //await _unitOfWork.SaveChangeAsync();
            if(!await _elderService.ElderExists(id))
            {
                return NotFound();
            }
            await _elderService.DeleteElderly(id);

            return NoContent();
        }

        //private async Task<bool> ElderExists(int id)
        //{
        //    return await _unitOfWork.ElderRepo.GetByIdAsync(id) != null;
        //}
    }
}

