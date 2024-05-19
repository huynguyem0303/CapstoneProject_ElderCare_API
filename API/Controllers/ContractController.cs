using API.DTO;
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

        // GET: api/Certifications
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetContracts()
        {
            var list = _contractService.FindAsync(e => true, e => e.ContractServices, e => e.ContractVersions);

            return Ok(list);
        }
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<SingleResult> GetContract(int id)
        {
            var contract = await _contractService.FindAsync(e => e.ContractId == id, e => e.ContractServices, e => e.ContractVersions);

            return SingleResult.Create(contract.AsQueryable());
        }

        /// <summary>
        /// Get contracts based on carerId
        /// </summary>
        /// <param name="carerId"></param>
        /// <returns></returns>
        [HttpGet("getContractByCarerId")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<IActionResult> GetContractByCarerId(int carerId)
        {
            var contract = await _contractService.FindAsync(e => e.CarerId == carerId, e => e.ContractServices, e => e.ContractVersions);
            if (!contract.IsNullOrEmpty())
            {
                return Ok(contract);
            }
            return NotFound();
        }

        /// <summary>
        /// Get contracts based on customerId
        /// </summary>
        /// <param name="cusid">customerId</param>
        /// <returns></returns>
        [HttpGet("getContractByCusId")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<IActionResult> GetContractByCusId(int cusid)
        {
            var contract = await _contractService.FindAsync(e => e.CustomerId == cusid, e => e.ContractServices, e => e.ContractVersions);
            if (!contract.IsNullOrEmpty())
            {
                return Ok(contract);
            }
            return NotFound();
        }

        /// <summary>
        /// Get pending contracts based on carerId
        /// </summary>
        /// <param name="carerid">carerId</param>
        /// <returns></returns>
        [HttpGet("getPendingContractByCarerId")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<IActionResult> GetPendingContractByCarerId(int carerid)
        {
            var contract = await _contractService.FindAsync(e => e.CarerId == carerid && e.Status == (int)ContractStatus.Pending, e => e.ContractServices, e => e.ContractVersions);
            if (!contract.IsNullOrEmpty())
            {
                return Ok(contract);
            }
            return NotFound();
        }

        /// <summary>
        /// Get pending contracts based on customer Id
        /// </summary>
        /// <param name="cusid">customerId</param>
        /// <returns></returns>
        [HttpGet("getPendingContractByCusId")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<IActionResult> GetPendingContractByCusId(int cusid)
        {
            var contract = await _contractService.FindAsync(e => e.CustomerId == cusid && e.Status == (int)ContractStatus.Pending,e => e.ContractServices, e => e.ContractVersions);
            if (!contract.IsNullOrEmpty())
            {
                return Ok(contract);
            }
            return NotFound();
        }

        /// <summary>
        /// Create contract
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<ActionResult<Contract>> PostContract(AddContractDto dto)
        {
            Contract contract;
            try
            {
                contract = await _contractService.AddContract(dto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            if (contract != null)
            {
                return Ok(contract);
            }
            return NotFound();
        }

        /// <summary>
        /// Create contract with tracking timetables
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateWithTimetable")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Carer, Staff")]
        public async Task<ActionResult<Contract>> PostContract2(AddContractWithTrackingsDto dto)
        {
            Contract contract;
            var trackingTimeables = new List<Timetable>();
            try
            {
                (contract, trackingTimeables) = await _contractService.AddContract2(dto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }
            if (contract != null && !trackingTimeables.IsNullOrEmpty())
            {
                return Ok(new
                {
                    contract,
                    trackingTimeables
                });
            }
            return NotFound();
        }

        /// <summary>
        /// This method approved or denied customer's contract
        /// </summary>
        /// <param name="id">Contract id</param>
        /// <param name="status">Contract status:
        ///         0 - pending;
        ///         1 - Signed (send noti to customer and then create new transaction);
        ///         2 - Rejected(can be done by carer or customer when they dont want to make transaction)
        ///         3 - Active 
        ///         4 - Waiting for transaction
        ///         5 - Expired 
        ///         6 - CusComplained
        ///         7 - Terminated</param>
        /// <returns></returns>
        [HttpPut("{id}/Contract")]
        [EnableQuery]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ApproveContract(int id, ContractStatus status)
        {
            if (!await _contractService.ContractExists(id))
            {
                return BadRequest(error: "Khong co contract can tra luong cho carer!!");
            }

            var contract = await _contractService.ApproveContract(id, (int)status);

            if (contract != null)
            {
                return Ok(contract);
            }
            return NoContent();
        }

        /// <summary>
        /// Check and update expired contracts status
        /// </summary>
        /// <returns></returns>
        [HttpPut("ExpiredContract")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> ExpiredContract()
        {
            await _contractService.ExpriedContract();
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Checking xong"

            }); ;
        }

        /// <summary>
        /// Get all expired contract of today
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExpiredContractToday")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> ExpiredContractToday()
        {
            var list = await _contractService.ExpriedContractToday();
            if (list.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(list);
        }

        /// <summary>
        /// Get all expired contract in the next 5 days
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExpiredContractInNext5Day")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> ExpiredContractInNext5Day()
        {
            var list = await _contractService.ExpriedContractInNext5Day();
            if (list.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(list);
        }
    }
}
