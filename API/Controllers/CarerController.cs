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
using ElderCare_Service.Services;
using System.Data;

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
        [HttpGet("categoryname")]
        [EnableQuery]
        public async Task<IActionResult> GetCategoryByServiceName(string name)
        {
            //var carer = await _unitOfWork.AccountRepository.FindAsync(x => x.AccountId == id);
            var carer = await _carerService.FindCateAsync(x => x.ServiceName.Contains(name));
            return Ok(carer);
        }
        /// <summary>
        /// This method get all services of a carer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Services")]
        [EnableQuery]
        public async Task<IActionResult> GetServicesByCarerId(int id)
        {
            if (!await _carerService.CarerExists(id))
            {
                return NotFound();
            }
            var services = await _carerService.GetServicesByCarerId(id);
            return Ok(services);
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
        /// This method approved or denied carer's attempt to create account
        /// </summary>
        /// <param name="id">carer id</param>
        /// <param name="status">carer account status:
        ///         0 - pending;
        ///         1 - approved (create a new account if not exist);
        ///         2 - declined</param>
        /// <returns></returns>
        [HttpPut("{id}/Account")]
        [EnableQuery]
        public async Task<IActionResult> ApproveCarer(int id, CarerStatus status)
        {
            if(!await _carerService.CarerExists(id))
            {
                return NotFound();
            }

            var account = await _carerService.ApproveCarer(id, (int)status);

            if (account != null && account.Status == (int)AccountStatus.Active)
            {
                return Ok(account);
            }
            return NoContent();
        }

        /// <summary>
        /// This method add services to carer
        /// </summary>
        /// <param name="carerId"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [HttpPost("{carerId}/Services")]
        [EnableQuery]
        [Authorize]
        public async Task<ActionResult<Package>> PostCarerService(int carerId, string[] serviceName)
        {
            if (!await _carerService.CarerExists(carerId))
            {
                return NotFound();
            }
            List<CarerServiceDto> carerServices;
            try
            {
                carerServices = await _carerService.AddCarerServiceAsync(carerId, serviceName);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetCarerById", new { id = carerId }, carerServices); ;
        }

        /// <summary>
        /// This method remove service from carer
        /// </summary>
        /// <param name="carerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpDelete("{carerId}/Services/{serviceId}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> RemoveService(int carerId, int serviceId)
        {
            if (!await _carerService.CarerExists(carerId))
            {
                return NotFound();
            }
            try
            {
                await _carerService.RemoveCarerService(carerId, serviceId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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
