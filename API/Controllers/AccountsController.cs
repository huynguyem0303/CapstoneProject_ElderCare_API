using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using Microsoft.IdentityModel.Tokens;
using ElderCare_Repository.ViewModels;
using AutoMapper;
using API.Ultils;
using ElderCare_Repository.Repos;
using API.DTO;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Accounts
        [HttpGet]
        [Authorize(Roles="Staff, Admin")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var list = await _unitOfWork.AccountRepository.GetAllAsync();
          if (list.IsNullOrEmpty())
          {
              return NotFound();
          }
            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
          if ((await _unitOfWork.AccountRepository.GetAllAsync()).IsNullOrEmpty())
          {
              return NotFound();
          }
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            await _unitOfWork.AccountRepository.UpdateAsync(account);

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AccountExists(id))
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
        public async Task<ActionResult<Account>> PostAccount(SignInViewModel model)
        {
            if ((await _unitOfWork.AccountRepository.GetAllAsync()).IsNullOrEmpty())
            {
                return Problem("Entity set 'ElderCareContext.Accounts'  is null.");
            }
            var account = _mapper.Map<Account>(model);
            await _unitOfWork.AccountRepository.AddAsync(account);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateException)
            {
                if (await AccountExists(account.AccountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if ((await _unitOfWork.AccountRepository.GetAllAsync()).IsNullOrEmpty())
            {
                return NotFound();
            }
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            await _unitOfWork.AccountRepository.DeleteAsync(account);
            await _unitOfWork.SaveChangeAsync();

            return NoContent();
        }

        private async Task<bool> AccountExists(int id)
        {
            return await _unitOfWork.AccountRepository.GetByIdAsync(id) != null;
        }
        [HttpPost("loginCustomer")]
        public async Task<IActionResult> LoginCus(LoginDto loginDto)
        {
           IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
           // string adminEmail = config["AdminAccount:Email"];
           // string adminPassword = config["AdminAccount:Password"];
           // if (loginDto.email.ToLower().Equals(adminEmail.ToLower()) && loginDto.password.Equals(adminPassword))
           //     return Ok(new ApiResponse
           //     {
           //         Success = true,
           //         Message = "Authenticate success",
           //         Data = GenerateJWTString.GenerateJsonWebTokenForAdmin(adminEmail, config["AppSettings:SecretKey"], DateTime.Now)
           //     }); ;
            var account = await _unitOfWork.AccountRepository.LoginCustomerAsync(loginDto.email, loginDto.password);
            if (account == null)
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebToken(account, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }
        [HttpPost("loginCarer")]
        public async Task<IActionResult> LoginCarer(LoginDto loginDto)
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            var account = await _unitOfWork.AccountRepository.LoginCarerAsync(loginDto.email, loginDto.password);
            if (account == null)
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebTokenForCarer(account, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }
        [HttpPost("loginStaff")]
        public async Task<IActionResult> LoginStaff(LoginDto loginDto)
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            var account = await _unitOfWork.AccountRepository.LoginStaffAsync(loginDto.email, loginDto.password);
            if (account == null)
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebTokenForStaff(account, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }
    }
}
