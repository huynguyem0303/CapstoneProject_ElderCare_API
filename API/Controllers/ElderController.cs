using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElderController  : ODataController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ElderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/Accounts
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetElders()
        {
            var list = _unitOfWork.ElderRepo.GetAll();

            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Elderly>> GetElder(int id)
        {
            var elder = await _unitOfWork.ElderRepo.FindAsync(x => x.ElderlyId == id);
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

            _unitOfWork.ElderRepo.Update(elder);

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ElderExists(id))
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

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<Account>> PostElder(AddElderDto model)
        {
            if ((_unitOfWork.AccountRepository.GetAll()).IsNullOrEmpty())
            {
                return Problem("Entity set 'ElderCareContext.Elderlies'  is null.");
            }
            var elder = _mapper.Map<Elderly>(model);
            var id = _unitOfWork.ElderRepo.GetAll().OrderByDescending(i => i.ElderlyId).FirstOrDefault().ElderlyId;
            elder.ElderlyId = id+1;
            await _unitOfWork.ElderRepo.AddAsync(elder);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateException)
            {
                if (await ElderExists(elder.ElderlyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElder", new { id = elder.ElderlyId }, elder);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> DeleteElder(int id)
        {
            if ((_unitOfWork.ElderRepo.GetAll()).IsNullOrEmpty())
            {
                return NotFound();
            }
            var elder = await _unitOfWork.ElderRepo.GetByIdAsync(id);
            if (elder == null)
            {
                return NotFound();
            }

            _unitOfWork.ElderRepo.Delete(elder);
            await _unitOfWork.SaveChangeAsync();

            return NoContent();
        }

        private async Task<bool> ElderExists(int id)
        {
            return await _unitOfWork.ElderRepo.GetByIdAsync(id) != null;
        }
    }
}

