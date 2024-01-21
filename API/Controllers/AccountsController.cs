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
            var list = await _unitOfWork.AccountRepository.GetAllAsync();
            if (list.IsNullOrEmpty())
            {
                return Problem("Entity set 'ElderCareContext.Accounts'  is null.");
            }
            var account = _mapper.Map<Account>(model);
            account.AccountId = list.Last().AccountId+1;
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
    }
}
