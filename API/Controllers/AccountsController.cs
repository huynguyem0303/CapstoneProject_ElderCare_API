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
using AutoMapper;
using API.Ultils;
using ElderCare_Repository.Repos;
using API.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ElderCare_Repository.DTO;
using System.Data;
using ElderCare_Domain.Enums;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ODataController
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
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public IActionResult GetAccounts()
        {
            var list = _unitOfWork.AccountRepository.GetAll();
            
            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Account>> GetAccount(int id)
        {
            var account = await _unitOfWork.AccountRepository.FindAsync(x=>x.AccountId == id);
            return SingleResult.Create(account.AsQueryable());
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            _unitOfWork.AccountRepository.Update(account);

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
        [EnableQuery]
        public async Task<ActionResult<Account>> PostAccount(SignInDto model)
        {
            if ((_unitOfWork.AccountRepository.GetAll()).IsNullOrEmpty())
            {
                return Problem("Entity set 'ElderCareContext.Accounts'  is null.");
            }
            var account = _mapper.Map<Account>(model);
            account.Status = (int)AccountStatus.Active;
            account.RoleId = (int)AccountRole.None;
            try
            {
                await _unitOfWork.AccountRepository.AddAsync(account);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (DbUpdateException e)
            {
                if (await AccountExists(account.AccountId))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(error: e.Message);
                }
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if ((_unitOfWork.AccountRepository.GetAll()).IsNullOrEmpty())
            {
                return NotFound();
            }
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _unitOfWork.AccountRepository.Delete(account);
            await _unitOfWork.SaveChangeAsync();

            return NoContent();
        }

        private async Task<bool> AccountExists(int id)
        {
            return await _unitOfWork.AccountRepository.GetByIdAsync(id) != null;
        }
    }
}
