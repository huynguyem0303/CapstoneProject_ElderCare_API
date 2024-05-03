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
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;
        private readonly ISignupService _signupService;

        public SignupController(ISignupService signupService)
        {
            _signupService = signupService;
        }

        //public SignInController(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}

        /// <summary>
        /// Signup new carer
        /// </summary>
        /// <param name="carerDto"> field "certificationType" (DO NOT LEAVE IT AT 0): 
        /// certificate id in table Certification, get Certification: api/Certifications</param>
        /// <returns></returns>
        [HttpPost("signinCarer")]
        public async Task<IActionResult> SignUpCarer(CarerSignUpDto carerDto)
        {
            //var carer = _mapper.Map<Carer>(carerDto);
            //var account = _mapper.Map<Account>(carerDto);
            //carer = await _unitOfWork.CarerRepository.AddAsync(carer);
            //account.CarerId = carer.CarerId;
            //account.RoleId = (int)AccountRole.Carer;
            //account.Status = (int)AccountStatus.Active;
            //Guid guid = Guid.NewGuid();
            //string randomString = guid.ToString("N").Substring(0, 10);
            //account.Password = randomString;
            //await _unitOfWork.AccountRepository.AddAsync(account);
            Carer carer;
            try
            {
                //await _unitOfWork.SaveChangeAsync();
                carer = await _signupService.SignUpCarer(carerDto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            return CreatedAtAction("GetCarerById", controllerName: "Carer", new { id = carer.CarerId }, carer);
        }

        [HttpPost("signinCustomer")]
        public async Task<IActionResult> SignUpCustomer(CustomerSignUpDto customerDto)
        {
            //var customer = _mapper.Map<Customer>(customerDto);
            //var account = _mapper.Map<Account>(customerDto);
            //customer = await _unitOfWork.CustomerRepository.AddAsync(customer);
            //account.CustomerId = customer.CustomerId;
            //account.RoleId = (int)AccountRole.Customer;
            //account.Status = (int)AccountStatus.Active;
            //await _unitOfWork.AccountRepository.AddAsync(account);
            Account account;
            try
            {
                //await _unitOfWork.SaveChangeAsync();
                account = await _signupService.SignUpCustomer(customerDto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(error: e.Message);
            }
            return CreatedAtAction("GetAccount", controllerName: "Accounts", new { id = account.AccountId }, account);
        }
    }
}
