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

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult> GetContractByCarerId([FromRoute] int carerid)
        {
            //var model = await _unitOfWork.ElderRepo.FindAsync(x => x.ElderlyId == elderId);
            var elder = await _contractService.FindAsync(e => e.CarerId == carerid && e.Status==(int)ContractStatus.Pending); 
            return SingleResult.Create(elder.AsQueryable());
        }
        [HttpPost]
        [EnableQuery]
        public async Task<ActionResult<Contract>> PostContract(int cusid, int carerid, int elderlyid, DateTime startDate, DateTime endDate, string package, string[] service)
        {
            Contract contract;
            try
            {
                //await _unitOfWork.SaveChangeAsync();
                contract = await _contractService.AddContract(cusid, carerid, elderlyid, startDate, endDate, package, service);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("GetContractByCarerId",  contract.CarerId , contract);
        }
    }
}
