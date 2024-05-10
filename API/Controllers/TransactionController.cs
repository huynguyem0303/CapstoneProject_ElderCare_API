using API.DTO;
using API.Ultils;
using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Service;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ODataController
    {
        private readonly ICarerService _carerService;
        private readonly IAccountService _accountService;


        private readonly ITransactionService _transactionService;
        public static string? url;

        public TransactionController(ICarerService carerService, IAccountService accountService, ITransactionService transactionService)
        {
            _carerService = carerService;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "Staff")]
        public IActionResult GetAllTransactions()
        {
            var list = _transactionService.GetAll();

            return Ok(list);
        }

        [HttpPost]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto dto,int carerid,int customerid,int contractid)
        {
            
            var idClaim = _accountService.GetMemberIdFromToken(HttpContext.User);
            var account = await _accountService.GetByIdAsync(idClaim);
            try
            {
                string paymentUrl = await _transactionService.CreateTransaction(dto, (int)account.AccountId,carerid, customerid,contractid);
                url= paymentUrl;
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Create success",
                    Data = paymentUrl
                }); ;
            }
            catch
            {
                return BadRequest("Error!");
            }
        }

        [HttpGet("/link-payment")]
        [Authorize]
        public async Task<IActionResult> LinkPayment()
        {
            try
            {
                var idClaim = _accountService.GetMemberIdFromToken(HttpContext.User);
                var userid = await _accountService.GetByIdAsync(idClaim);
                string paymentUrl =  _transactionService.LinkPayment((int)userid.AccountId);

                return Ok(paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/process-payment")]
        [Authorize]
        public async Task<IActionResult> ProcessPayment()
        {
            string returnContent = string.Empty;

            try
            {
                returnContent = await _transactionService.ProcessPayment();
            }
            catch (Exception)
            {
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"An error occurred\"}";
            }

            return Redirect(url);
        }

        [HttpGet("getTransactionHistoryByCarerId")]
        [EnableQuery]
        [Authorize(Roles = "Carer, Staff")]

        public async Task<IActionResult> GetTransactionHistoryByCarerId(int carerId)
        {
            try
            {
                var carerTransactions = await _carerService.GetTransactionHistoryByCarerIdAsync(carerId);
                if (carerTransactions.IsNullOrEmpty())
                {
                    return NotFound();
                }
                return Ok(carerTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getTransactionHistoryByCustomerId")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Staff")]
        public async Task<IActionResult> GetTransactionHistoryByCusId(int customerId)
        {
            try
            {
                var customerTransactions = await _carerService.GetTransactionHistoryByCustomerIdAsync(customerId);
                if (customerTransactions.IsNullOrEmpty())
                {
                    return NotFound();
                }
                return Ok(customerTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getApproveCarerCusByCustomerId")]
        [EnableQuery]
        [Authorize(Roles = "Customer, Staff")]
        public async Task<IActionResult> GetApproveCarerCusByCustomerId(int customerId)
        {
            try
            {
                var customerTransactions = await _carerService.GetCarerCusByCusId(customerId);
                if (customerTransactions.IsNullOrEmpty())
                {
                    return NotFound();
                }
                return Ok(customerTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getApproveCarerCusByCarerId")]
        [EnableQuery]
        [Authorize(Roles = "Carer, Staff")]
        public async Task<IActionResult> GetApproveCarerCusByCarerId(int carerId)
        {
            try
            {
                var carerTransactions = await _carerService.GetCarerCusByCarerId(carerId);
                if (carerTransactions.IsNullOrEmpty())
                {
                    return NotFound();
                }
                return Ok(carerTransactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CarerSalary")]
        [EnableQuery]
        [Authorize(Roles = "Carer, Staff")]
        public async Task<IActionResult> CarerSalary()
        {
          
            var list =await _transactionService.TransactionContract();
            if (list.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(list);
        }
    }

}

