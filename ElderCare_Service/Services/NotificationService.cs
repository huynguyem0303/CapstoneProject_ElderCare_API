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
using ExpoCommunityNotificationServer.Client;
using ExpoCommunityNotificationServer.Models;
using ExpoCommunityNotificationServer.Exceptions;
using ElderCare_Domain.Models;
using Microsoft.Extensions.Configuration;
namespace ElderCare_Service.Services
{

    public class NotificationService : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public NotificationService(IOptions<FcmNotificationSetting> settings, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _fcmNotificationSetting = settings.Value;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _config = config;
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
                if((await _unitOfWork.AccountRepository.FindAsync(e => e.Status != (int)AccountStatus.InActive
                                                            && e.AccountId == accountNotiModel.AccountId))
                                                            .IsNullOrEmpty())
                {
                    response.IsSuccess = false;
                    response.Message = "This account is unavailable";
                    return response;
                }
                var fcmTokens = await _unitOfWork.AccountRepository.GetDevicesByAccountId(accountNotiModel.AccountId);
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
                    notificationModel.DeviceId = fcmTokens[i].DeviceToken;
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
        public async Task<PushTicketResponse> SendExpoNotification(PushTicketRequestDto[] pushTicketReq)
        {
            var expoSDKClient = new PushApiClient();
            expoSDKClient.SetToken(_config["ExpoNotification:Token"]);
            PushTicketRequest[] request = _mapper.Map<PushTicketRequest[]>(pushTicketReq);
            //foreach(var item in request)
            //{
            //    item.PushPriority ??= "default";
            //    if (item.PushPriority != "default" && item.PushPriority != "normal" && item.PushPriority != "high")
            //    {
            //        throw new Exception("The delivery priority of the message. " +
            //            "Specify 'default' or omit this field to use the default priority on each platform ('normal' on Android and 'high' on iOS))");
            //    }
            //    item.PushSound ??= "default";
            //    if (item.PushSound != "default")
            //    {
            //        throw new Exception("Specify 'default' to play the device's default notification sound, or omit this field to play no sound");
            //    }
            //    item.PushChannelId ??= "default";
            //    item.PushTTL = 0;
            //}
            if (expoSDKClient.IsTokenSet())
            {
                var result = await expoSDKClient.SendPushAsync(request);
                return result;
            }
            return new PushTicketResponse();
        }

        public async Task<PushReceiptResponse> GetExpoNotificationReceipt(PushReceiptRequest pushReceiptReq)
        {
            var expoSDKClient = new PushApiClient();
            var result = await expoSDKClient.GetReceiptsAsync(pushReceiptReq);
            return result;
        }

        public async Task<PushTicketResponse> SendExpoNotificationToAccount(AccountExpoNotiDto accountNotiModel)
        {
            var response = new PushTicketResponse
            {
                Errors = new()
            };
            var requestModel = _mapper.Map<PushTicketRequest>(accountNotiModel.Data); 
            var expoSDKClient = new PushApiClient();
            expoSDKClient.SetToken(_config["ExpoNotification:Token"]);
            if ((await _unitOfWork.AccountRepository.FindAsync(e => e.Status != (int)AccountStatus.InActive
                                                            && e.AccountId == accountNotiModel.AccountId))
                                                            .IsNullOrEmpty() )
            {
                response.Errors.Add(new Error() { ErrorMessage = "This account is unavailable" });
                return response;
            }
            var devices = await _unitOfWork.AccountRepository.GetDevicesByAccountId(accountNotiModel.AccountId);
            if (devices.IsNullOrEmpty())
            {
                response.Errors.Add(new Error() { ErrorMessage = "Can't find any device on this account" });
                return response;
            }
            requestModel.PushTo = devices.Select(e => e.DeviceToken).ToList();
            if (expoSDKClient.IsTokenSet())
            {
                response = await expoSDKClient.SendPushAsync(requestModel);
                var notification = _mapper.Map<Notification>(accountNotiModel.Data);
                notification.AccountId = accountNotiModel.AccountId;
                await _unitOfWork.NotificationRepo.AddAsync(notification);
            }
            if(response.Errors.IsNullOrEmpty())
            {
                await _unitOfWork.SaveChangeAsync();
            }
            return response;
        }

        public IEnumerable<Notification> GetAll()
        {
            return _unitOfWork.NotificationRepo.GetAll();
        }
    }
}
