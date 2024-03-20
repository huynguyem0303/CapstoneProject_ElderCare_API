using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
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
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpGet("{carerid}",Name = "GetContractByCarerId")]
        [EnableQuery]
        public async Task<IActionResult> GetContractByCarerId([FromRoute] int carerid)
        {
            //var model = await _unitOfWork.ElderRepo.FindAsync(x => x.ElderlyId == elderId);
            var contract = await _contractService.FindAsync(e => e.CarerId == carerid && e.Status==(int)ContractStatus.Pending);
            if (!contract.IsNullOrEmpty())
            {
                return Ok(contract);
            }
            return NotFound();
        }
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<Contract>> PostContract(AddContractDto dto)
        {
            Contract contract;
            try
            {
                //await _unitOfWork.SaveChangeAsync();
                contract = await _contractService.AddContract(dto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("GetContractByCarerId", new { id = contract.CarerId}, contract);
        }    /// <summary>
             /// This method approved or denied customer's contract
             /// </summary>
             /// <param name="id">Contract id</param>
             /// <param name="status">Contract status:
             ///         0 - pending;
             ///         1 - Signed (send noti to customer and then create new transaction);
             ///         2 - Rejected(can be done by carer or customer when they dont want to make transaction)
             ///         3 - Active 
             ///         4 - Expired </param>
             /// <returns></returns>
        [HttpPut("{id}/Contract")]
        [EnableQuery]
        public async Task<IActionResult> ApproveCarer(int id, ContractStatus status)
        {
            if (!await _contractService.ContractExists(id))
            {
                return NotFound();
            }

            var contract = await _contractService.ApproveContract(id, (int)status);

            if (contract != null)
            {
                return Ok(contract);
            }
            return NoContent();
        }
    }
}
