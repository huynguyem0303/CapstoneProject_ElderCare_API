using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SignInController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("signinCarer")]
        public async Task<IActionResult> SignInCarer(CarerSignInDto model)
        {
            var carer = _mapper.Map<Carer>(model);
            var account = _mapper.Map<Account>(model);
            carer = await _unitOfWork.CarerRepository.AddAsync(carer);
            account.CarerId = carer.CarerId;
            account.RoleId = (int)AccountRole.Carer;
            account.Status = (int)AccountStatus.Active;
            Guid guid = Guid.NewGuid();
            string randomString = guid.ToString("N").Substring(0, 10);
            account.Password = randomString;
            await _unitOfWork.AccountRepository.AddAsync(account);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            return CreatedAtAction("GetAccount", controllerName: "Accounts", new { id = account.AccountId }, account);
        }
    }
}
