﻿using API.DTO;
using API.Ultils;
using AutoMapper;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("loginCustomer")]
        public async Task<IActionResult> LoginCus(LoginDto loginDto)
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            // string adminEmail = config["AdminAccount:Email"];
            // string adminPassword = config["AdminAccount:Password"];
            // if (loginDto.Email.ToLower().Equals(adminEmail.ToLower()) && loginDto.Password.Equals(adminPassword))
            //     return Ok(new ApiResponse
            //     {
            //         Success = true,
            //         Message = "Authenticate success",
            //         Data = GenerateJWTString.GenerateJsonWebTokenForAdmin(adminEmail, config["AppSettings:SecretKey"], DateTime.Now)
            //     }); ;
            var account = await _unitOfWork.AccountRepository.LoginCustomerAsync(loginDto.Email, loginDto.Password);
            if (account == null)
            {
                return Unauthorized();
            }
            else if(!loginDto.FCMToken.IsNullOrEmpty())
            {
                var response = await checkAccountFCMToken(account.AccountId, loginDto.FCMToken);
                if (!response.IsSuccess)
                {
                    return Ok(response);
                }
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebToken(account, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }
        [HttpPost("loginCarer")]
        public async Task<IActionResult> LoginCarer(LoginDto loginDto)
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            var account = await _unitOfWork.AccountRepository.LoginCarerAsync(loginDto.Email, loginDto.Password);
            if (account == null)
            {
                return Unauthorized();
            }
            else if (!loginDto.FCMToken.IsNullOrEmpty())
            {
                var response = await checkAccountFCMToken(account.AccountId, loginDto.FCMToken);
                if (!response.IsSuccess)
                {
                    return Ok(response);
                }
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebTokenForCarer(account, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }
        [HttpPost("loginStaff")]
        public async Task<IActionResult> LoginStaff(LoginDto loginDto)
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            var account = await _unitOfWork.AccountRepository.LoginStaffAsync(loginDto.Email, loginDto.Password);
            if (account == null)
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebTokenForStaff(account, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }

        private async Task<ResponseModel> checkAccountFCMToken(int accountId, string token)
        {
            var response = await _unitOfWork.NotificationService.SendNotification(new ElderCare_Domain.Commons.NotificationModel()
            {
                Title = "Login Notification",
                IsAndroidDevice = true,
                Body = "An account attemp to login into this device",
                DeviceId = token,
            });
            if (response.IsSuccess)
            {
                await _unitOfWork.AccountRepository.AddFCMToken(accountId, token);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                response.Message = $"Invalid FCMToken: {response.Message}";
            }
            return response;
        }
    }
}