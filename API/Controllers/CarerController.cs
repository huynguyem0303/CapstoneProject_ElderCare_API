using API.DTO;
using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Service;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ElderCare_Service.Interfaces;
using ElderCare_Domain.Enums;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarerController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;
        private readonly ICarerService _carerService;

        public CarerController(ICarerService carerService)
        {
            _carerService = carerService;
        }

        //public CarerController(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}

        [HttpPost("search")]
        public async Task<IActionResult> GetCarer(SearchCarerDto dto)
        {
            //var carer = await _unitOfWork.CarerRepository.SearchCarer(dto);
            var carer = await _carerService.SearchCarer(dto);
            if (!carer.IsNullOrEmpty())
            {
                return Ok(carer);
            }
            return NotFound();
        }
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult<Carer>> GetCarerById(int id)
        {
            //var carer = await _unitOfWork.AccountRepository.FindAsync(x => x.AccountId == id);
            var carer = await _carerService.FindAsync(x => x.CarerId == id);
            return SingleResult.Create(carer.AsQueryable());
        }
        [HttpGet("pending")]
        [EnableQuery]
        public async Task<IActionResult> GetPendingCarer()
        {
            var carer = await _carerService.GetByPending();
            if (!carer.IsNullOrEmpty())
            {
                return Ok(carer);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        [EnableQuery]
        public async Task<IActionResult> PutCarer(int id, Carer carer)
        {
            if (id != carer.CarerId)
            {
                return BadRequest();
            }

            //_unitOfWork.CarerRepository.Update(carer);

            try
            {
                //await _unitOfWork.SaveChangeAsync();
                await _carerService.UpdateCarer(carer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _carerService.CarerExists(id))
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

        /// <summary>
        /// This method update status in the account table status with the carer id
        /// </summary>
        /// <param name="id">carer id</param>
        /// <param name="status">carer account status:
        ///         0 - pending;
        ///         1 - approved;
        ///         2 - declined</param>
        /// <returns></returns>
        [HttpPut("{id}/AccountStatus")]
        [EnableQuery]
        public async Task<IActionResult> ChangeCarerAccountStatus(int id, CarerStatus status)
        {
            if(!await _carerService.CarerExists(id))
            {
                return NotFound();
            }

            await _carerService.ChangeCarerAccountStatus(id, (int)status);

            return NoContent();
        }

        //[HttpGet("getTransactionHistory")]
        //[EnableQuery]
        //[Authorize(Roles = "Carer")]
        //public async Task<IActionResult> GetCarerTransactionHistory(int carerId)
        //{
        //    try
        //    {
        //        //var transactionList = await _unitOfWork.CarerRepository.GetCarerTransactionHistoryAsync(carerId);
        //        //var carerTransactions = _mapper.Map<List<CarerTransactionDto>>(transactionList);
        //        //foreach (var transaction in carerTransactions)
        //        //{
        //        //    var carerCus = await _unitOfWork.CarerRepository.GetCarerCustomerFromIdAsync(transactionList[carerTransactions.IndexOf(transaction)].CarercusId);
        //        //    if(carerCus != null)
        //        //    {
        //        //        (transaction.CarerId, transaction.CustomerId) = (carerCus.CarerId, carerCus.CustomerId);
        //        //    }
        //        //}
        //        var carerTransactions = await _carerService.GetCarerTransactionHistoryAsync(carerId);
        //        return Ok(carerTransactions);
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //private async Task<bool> CarerExists(int id)
        //{
        //    return await _unitOfWork.CarerRepository.GetByIdAsync(id) != null;
        //}
    }
}
