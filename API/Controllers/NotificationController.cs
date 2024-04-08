using ElderCare_Domain.Commons;
using ElderCare_Service.Interfaces;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tsp;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ODataController
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
        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            //var result = await _unitOfWork.NotificationService.SendNotification(notificationModel);
            var result = await _notificationService.SendNotification(notificationModel);
            return Ok(result);
        }

        /// <summary>
        /// send noti using expo services, please send expo token when login to use this function
        /// </summary>
        /// <param name="accountNotiDto"></param>
        /// <returns></returns>
        [Route("sendToAccount")]
        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAccount(AccountExpoNotiDto accountNotiDto)
        {
            //var result = await _unitOfWork.NotificationService.SendNotificationToAccount(accountNotiDto);
            //var result = await _notificationService.SendNotificationToAccount(accountNotiDto);
            var result = await _notificationService.SendExpoNotificationToAccount(accountNotiDto);
            return Ok(result);
        }
        /// <summary>
        /// send noti using expo service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("ExpoNoti")]
        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> SendExpoNotification(PushTicketRequestDto[] request)
        {
            try
            {

                var result = await _notificationService.SendExpoNotification(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// recieve noti reciept after sending noti
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("ExpoNotiReceipt")]
        [EnableQuery]
        [HttpPost]
        public async Task<IActionResult> GetExpoNotificationReceipt(List<string> ids)
        {
            var result = await _notificationService.GetExpoNotificationReceipt(new ExpoCommunityNotificationServer.Models.PushReceiptRequest() { PushTicketIds = ids});
            return Ok(result);
        }

        [EnableQuery]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications()
        {
            var result = _notificationService.GetAll();
            return Ok(result);
        }
    }
}
