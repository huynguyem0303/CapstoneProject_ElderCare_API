using ElderCare_Domain.Models;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : Controller
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult GetPackages()
        {
            var list = _packageService.GetAll();
            return Ok(list);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<SingleResult<Package>> GetPackage(int id)
        {
            var account = await _packageService.FindAsync(x => x.PackageId == id);
            return SingleResult.Create(account.AsQueryable());
        }
    }
}
