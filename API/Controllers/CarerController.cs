using API.DTO;
using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
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

    }
}
