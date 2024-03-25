using ElderCare_Domain.Models;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetServices()
        {
            var list = _serviceService.GetAll();
            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult<Service>> GetService(int id)
        {
            var account = await _serviceService.FindAsync(x => x.ServiceId == id);
            return SingleResult.Create(account.AsQueryable());
        }
    }
}
