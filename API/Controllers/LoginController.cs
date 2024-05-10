using API.DTO;
using API.Ultils;
using AutoMapper;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Models;
using ElderCare_Service;
using ElderCare_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public static string? currentJWT;
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("loginCustomer")]
        public async Task<IActionResult> LoginCus(LoginDto loginDto)
        {
            try
            {
                currentJWT = await _loginService.LoginCusAsync(loginDto.Email, loginDto.Password, loginDto.DeviceToken);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = currentJWT
            });
        }
        [HttpPost("loginCarer")]
        public async Task<IActionResult> LoginCarer(LoginDto loginDto)
        {
            try
            {
                currentJWT = await _loginService.LoginCarerAsync(loginDto.Email, loginDto.Password, loginDto.DeviceToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = currentJWT
            });
            ;
        }
        [HttpPost("loginStaff")]
        public async Task<IActionResult> LoginStaff(LoginDto loginDto)
        {
            try
            {
                currentJWT = await _loginService.LoginStaffAsync(loginDto.Email, loginDto.Password, loginDto.DeviceToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = currentJWT
            }); ;
        }
    }
}
