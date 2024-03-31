using AutoMapper;
using CorePush.Apple;
using ElderCare_Domain.Commons;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Utils;
using ExpoCommunityNotificationServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ElderCare_Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration config;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public static string? currentJWT;
        public LoginService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> LoginCusAsync(string email, string password, string DeviceToken)
        {
            var account = await _unitOfWork.AccountRepository.LoginCustomerAsync(email, password);
            if(account == null)
            {
                throw new UnauthorizedAccessException("incorrect email or password");
            }else if (!DeviceToken.IsNullOrEmpty())
            {
                //var response = await checkAccountFCMToken((int)account.AccountId, DeviceToken);
                var response = await checkAccountExpoToken((int)account.AccountId, DeviceToken);
                if (response?.Errors?.Count > 0)
                {
                    string message = "";
                    foreach (var error in response.Errors)
                    {
                        message += $"Error: {error.ErrorCode} - {error.ErrorMessage} \n";
                    }
                    throw new Exception(message);
                }
            }
            currentJWT = GenerateJWTString.GenerateJsonWebToken(account, config["AppSettings:SecretKey"], DateTime.Now);
            return currentJWT;
        }

        public async Task<string> LoginCarerAsync(string email, string password, string DeviceToken)
        {
            var account = await _unitOfWork.AccountRepository.LoginCarerAsync(email, password);
            if (account == null)
            {
                throw new UnauthorizedAccessException("incorrect email or password");
            }
            else if (!DeviceToken.IsNullOrEmpty())
            {
                //var response = await checkAccountFCMToken((int)account.AccountId, DeviceToken);
                var response = await checkAccountExpoToken((int)account.AccountId, DeviceToken);
                if (response?.Errors?.Count > 0)
                {
                    string message = "";
                    foreach (var error in response.Errors)
                    {
                        message += $"Error: {error.ErrorCode} - {error.ErrorMessage} \n";
                    }
                    throw new Exception(message);
                }
            }
            currentJWT = GenerateJWTString.GenerateJsonWebTokenForCarer(account, config["AppSettings:SecretKey"], DateTime.Now);
            return currentJWT;
        }

        public async Task<string> LoginStaffAsync(string email, string password, string DeviceToken)
        {
            string adminEmail = config["AdminAccount:Email"];
            string adminPassword = config["AdminAccount:Password"];
            string adminId = config["AdminAccount:Id"];
            if (email.ToLower().Equals(adminEmail.ToLower()) && password.Equals(adminPassword))
            {
                return currentJWT = GenerateJWTString.GenerateJsonWebTokenForAdmin(adminEmail, config["AppSettings:SecretKey"], DateTime.Now, adminId);
            }
            var account = await _unitOfWork.AccountRepository.LoginStaffAsync(email, password);
            if (account == null)
            {
                throw new UnauthorizedAccessException("incorrect email or password");
            }
            else if (!DeviceToken.IsNullOrEmpty())
            {
                //var response = await checkAccountFCMToken((int)account.AccountId, DeviceToken);
                var response = await checkAccountExpoToken((int)account.AccountId, DeviceToken);
                if (response?.Errors?.Count > 0)
                {
                    string message = "";
                    foreach (var error in response.Errors)
                    {
                        message += $"Error: {error.ErrorCode} - {error.ErrorMessage} \n";
                    }
                    throw new Exception(message);
                }
            }
            currentJWT = GenerateJWTString.GenerateJsonWebTokenForCarer(account, config["AppSettings:SecretKey"], DateTime.Now);
            return currentJWT;
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
                await _unitOfWork.AccountRepository.AddDeviceToken(accountId, token);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                response.Message = $"Invalid Device: {response.Message}";
            }
            return response;
        }
        private async Task<PushTicketResponse> checkAccountExpoToken(int accountId, string token)
        {
            var response = await _unitOfWork.NotificationService.SendExpoNotification(new ElderCare_Repository.DTO.PushTicketRequestDto[]
            {
                new ElderCare_Repository.DTO.PushTicketRequestDto()
                {
                    To = new List<string>{token},
                    Title = "Login Notification",
                    body = "An account attemp to login into this device",
                    ChannelId = "test"
                }
            });
            if (response?.Errors?.Count == 0)
            {
                await _unitOfWork.AccountRepository.AddDeviceToken(accountId, token);
                await _unitOfWork.SaveChangeAsync();
            }
            return response;
        }
    }
}
