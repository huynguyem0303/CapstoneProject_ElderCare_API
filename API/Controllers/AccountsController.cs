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
    public class AccountsController : ODataController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public IActionResult GetAccounts()
        {
            //var list = _unitOfWork.AccountRepository.GetAll();
            var list = _accountService.GetAll();
            
            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Account>> GetAccount(int id)
        {
            var account = await _accountService.FindAsync(x => x.AccountId == id);
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
            try
            {
                await _accountService.UpdateAccount(account);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _accountService.AccountExists(id))
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
            Account account;
            try
            {
                account = await _accountService.AddAccountAsync(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
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
            if (await _accountService.AccountExists(id))
            {
                return NotFound();
            }
            await _accountService.DeleteAccount(id);
            return NoContent();
        }
    }
}
