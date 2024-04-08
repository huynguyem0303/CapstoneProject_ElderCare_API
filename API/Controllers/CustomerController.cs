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
    public class CustomersController : ODataController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetCustomers()
        {
            var list = _customerService.GetAll();

            return Ok(list);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerService.FindAsync(x => x.CustomerId == id);
            return SingleResult.Create(customer.AsQueryable());
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> PutCustomer(int id, UpdateCustomerDto customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                await _customerService.UpdateCustomer(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _customerService.CustomerExists(id))
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
        /// This method return carer by customer Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        // GET: api/Customers
        [HttpGet("{customerId}/Carers")]
        [EnableQuery]
        [Authorize]
        public IActionResult GetCarersByCustomerId(int customerId)
        {
            var list = _customerService.GetCarersByCustomerId(customerId);

            return Ok(list);
        }

        /// <summary>
        /// This method return notifications based on customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("{customerId}/Notifications")]
        [EnableQuery]
        [Authorize]
        public IActionResult GetNotificationsByCustomerId(int customerId)
        {
            var list = _customerService.GetNotificationsByCustomerId(customerId);

            return Ok(list);
        }
    }
}
