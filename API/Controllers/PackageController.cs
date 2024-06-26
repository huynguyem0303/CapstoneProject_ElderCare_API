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
using ElderCare_Service.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ODataController
    {
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        // GET: api/Packages
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> GetPackagesAsync()
        {
            var list = await _packageService.GetAllAsync();
            
            return Ok(list);
        }

        // GET: api/Packages/5
        [HttpGet("{id}")]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> GetPackage(int id)
        {
            var package = await _packageService.GetById(id);
            if(package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        // PUT: api/Packages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> PutPackage(int id, UpdatePackageDto package)
        {
            if (id != package.PackageId)
            {
                return BadRequest();
            }

            try
            {
                await _packageService.UpdatePackage(package);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _packageService.PackageExists(id))
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

        // POST: api/Packages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<ActionResult<Package>> PostPackage(AddPackageDto model)
        {
            Package package;
            try
            {
                var check = await _packageService.PackageNameExists(model.Name);
                if (check == true)
                {
                    return BadRequest(error: "Duplicate Name!!");
                }
                else
                {
                    package = await _packageService.AddPackageAsync(model);
                }
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetPackage", new { id = package.PackageId }, package);
        }

        // DELETE: api/Packages/5
        [HttpDelete("{id}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            if (!await _packageService.PackageExists(id))
            {
                return NotFound();
            }
            var check=_packageService.ContractPackageExists(id);
            if (check.Result==true)
            {
                return BadRequest(error: "Package is still in contract!!!");
            }
            var checkservice=_packageService.PackageServiceExisted(id);
            if (checkservice.Result == true)
            {
                return BadRequest(error: "Service is still in Package!!!");
            }
            await _packageService.DeletePackage(id);
            
            return NoContent();
        }

        /// <summary>
        /// This method add services to package
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [HttpPost("{packageId}/Services")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<ActionResult<Package>> PostPackageService(int packageId, string[] serviceName)
        {
            if (!await _packageService.PackageExists(packageId))
            {
                return NotFound();
            }
            List<PackageServiceDto> packageService;
            try
            {
                packageService = await _packageService.AddPackageServiceAsync(packageId, serviceName);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            catch (DuplicateNameException e)
            {
                return Conflict(error: e.Message);
            }

            return CreatedAtAction("GetPackage", new { id = packageId }, packageService); ;
        }

        /// <summary>
        /// this method remove service from package
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpDelete("{packageId}/Services/{serviceId}")]
        [EnableQuery]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> RemoveService(int packageId, int serviceId)
        {
            if (!await _packageService.PackageExists(packageId))
            {
                return NotFound();
            }
            try
            {
                await _packageService.RemoveServiceFromPackage(packageId, serviceId);
            }catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
