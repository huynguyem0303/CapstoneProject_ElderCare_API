using API.DTO;
using API.Ultils;
using AutoMapper;
using ElderCare_Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            // if (loginDto.email.ToLower().Equals(adminEmail.ToLower()) && loginDto.password.Equals(adminPassword))
            //     return Ok(new ApiResponse
            //     {
            //         Success = true,
            //         Message = "Authenticate success",
            //         Data = GenerateJWTString.GenerateJsonWebTokenForAdmin(adminEmail, config["AppSettings:SecretKey"], DateTime.Now)
            //     }); ;
            var account = await _unitOfWork.AccountRepository.LoginCustomerAsync(loginDto.email, loginDto.password);
            if (account == null)
            {
                return Unauthorized();
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
            var account = await _unitOfWork.AccountRepository.LoginCarerAsync(loginDto.email, loginDto.password);
            if (account == null)
            {
                return Unauthorized();
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
            var account = await _unitOfWork.AccountRepository.LoginStaffAsync(loginDto.email, loginDto.password);
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
    }
}
