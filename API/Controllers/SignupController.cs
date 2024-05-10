using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Service;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ElderCare_Service.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly ISignupService _signupService;

        public SignupController(ISignupService signupService)
        {
            _signupService = signupService;
        }

        /// <summary>
        /// Signup new carer
        /// </summary>
        /// <param name="carerDto"> field "certificationType" (DO NOT LEAVE IT AT 0): 
        /// certificate id in table Certification, get Certification: api/Certifications</param>
        /// <returns></returns>
        [HttpPost("signinCarer")]
        public async Task<IActionResult> SignUpCarer(CarerSignUpDto carerDto)
        {
            Carer carer;
            try
            {
                carer = await _signupService.SignUpCarer(carerDto);
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message);
            }
            return CreatedAtAction("GetCarerById", controllerName: "Carer", new { id = carer.CarerId }, carer);
        }

        [HttpPost("signinCustomer")]
        public async Task<IActionResult> SignUpCustomer(CustomerSignUpDto customerDto)
        {
            Account account;
            try
            {
                account = await _signupService.SignUpCustomer(customerDto);
            }
            catch (Exception e)
            {
                return BadRequest(error: e.Message);
            }
            return CreatedAtAction("GetAccount", controllerName: "Accounts", new { id = account.AccountId }, account);
        }
    }
}
