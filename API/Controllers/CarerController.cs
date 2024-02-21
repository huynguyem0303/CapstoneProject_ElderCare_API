using API.DTO;
using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("search")]
        public async Task<IActionResult> GetCarer(SearchCarerDto dto)
        {
            var account = await _unitOfWork.CarerRepository.searchCarer(dto);

            return Ok(account);
        }
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult<Account>> GetCarerById(int id)
        {
            var account = await _unitOfWork.AccountRepository.FindAsync(x => x.AccountId == id);
            return SingleResult.Create(account.AsQueryable());
        }

        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutCarer(int id, Carer carer)
        {
            if (id != carer.CarerId)
            {
                return BadRequest();
            }

            _unitOfWork.CarerRepository.Update(carer);

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CarerExists(id))
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

        private async Task<bool> CarerExists(int id)
        {
            return await _unitOfWork.CarerRepository.GetByIdAsync(id) != null;
        }
    }
}
