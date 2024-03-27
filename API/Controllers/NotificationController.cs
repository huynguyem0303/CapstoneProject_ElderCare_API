using ElderCare_Domain.Commons;
using ElderCare_Service.Interfaces;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tsp;
using Expo.Server.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        //public NotificationController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}
        /// <summary>
        /// send noti using fcm services
        /// </summary>
        /// <param name="notificationModel"></param>
        /// <returns></returns>
        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            //var result = await _unitOfWork.NotificationService.SendNotification(notificationModel);
            var result = await _notificationService.SendNotification(notificationModel);
            return Ok(result);
        }

        /// <summary>
        /// send noti using fcm services, please send fcm token when login to using this function
        /// </summary>
        /// <param name="accountNotiDto"></param>
        /// <returns></returns>
        [Route("sendToAccount")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAccount(AccountNotiDto accountNotiDto)
        {
            //var result = await _unitOfWork.NotificationService.SendNotificationToAccount(accountNotiDto);
            var result = await _notificationService.SendNotificationToAccount(accountNotiDto);
            return Ok(result);
        }
        /// <summary>
        /// send noti using expo service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("ExpoNoti")]
        [HttpPost]
        public async Task<IActionResult> SendExpoNotification(PushTicketRequestDto request)
        {
            var result = await _notificationService.SendExpoNotification(request);
            return Ok(result);
        }

        /// <summary>
        /// recieve noti reciept after sending noti
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("ExpoNotiReceipt")]
        [HttpPost]
        public async Task<IActionResult> GetExpoNotification(List<string> ids)
        {
            var result = await _notificationService.GetExpoNotificationReceipt(new PushReceiptRequest() { PushTicketIds = ids});
            return Ok(result);
        }
    }
}
