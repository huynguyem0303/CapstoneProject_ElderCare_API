using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using ElderCare_Domain.Commons;
using static ElderCare_Domain.Commons.GoogleNotification;
using CorePush.Google;
using ElderCare_Repository.Interfaces;
using ElderCare_Repository.DTO;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ElderCare_Domain.Enums;
using ElderCare_Service.Interfaces;
namespace ElderCare_Service.Services
{

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public NotificationService(IOptions<FcmNotificationSetting> settings, IAccountRepository accountRepository, IMapper mapper)
        {
            _fcmNotificationSetting = settings.Value;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            var response = new ResponseModel();
            try
            {
                if (notificationModel.IsAndroidDevice)
                {
                    /* FCM Sender (Android Device) */
                    FcmSettings settings = new FcmSettings();
                    settings.SenderId = _fcmNotificationSetting.SenderId;
                    settings.ServerKey = _fcmNotificationSetting.ServerKey;
                    var httpClient = new HttpClient();

                    string authorizationKey = string.Format("keyy={0}", settings.ServerKey);
                    string deviceToken = notificationModel.DeviceId;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                   var dataPayload = new DataPayload();
                    dataPayload.Title = notificationModel.Title;
                    dataPayload.Body = notificationModel.Body;

                    var notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }
                }
                else
                {
                    /* Code here for APN Sender (iOS Device) */
                    //var apn = new ApnSender(apnSettings, httpClient);
                    //await apn.SendAsync(notification, deviceToken);
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public async Task<ResponseModel> SendNotificationToAccount(AccountNotiDto accountNotiModel)
        {
            var response = new ResponseModel();
            try
            {
                var notificationModel = _mapper.Map<NotificationModel>(accountNotiModel);
                if((await _accountRepository.FindAsync(e => e.Status != (int)AccountStatus.InActive
                                                            && e.AccountId == accountNotiModel.AccountId))
                                                            .IsNullOrEmpty())
                {
                    response.IsSuccess = false;
                    response.Message = "This account is unavailable";
                    return response;
                }
                var fcmTokens = await _accountRepository.GetFCMTokensByAccountId(accountNotiModel.AccountId);
                var resultList = new List<ResponseModel>();
                string message = "";
                if (fcmTokens.IsNullOrEmpty())
                {
                    response.IsSuccess = false;
                    response.Message = "The account is not logged in on any device";
                    return response;
                }
                for(var i=0; i < fcmTokens.Count(); i++)
                {
                    notificationModel.DeviceId = fcmTokens[i].DeviceFcmToken;
                    var result = await SendNotification(notificationModel);
                    resultList.Add(result);
                    message += $"device [{i+1}]: {result.Message}, ";
                }
                response.IsSuccess = !resultList.Any(e => !e.IsSuccess);
                response.Message = message;
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
            
            return response;
        }
    }
}
