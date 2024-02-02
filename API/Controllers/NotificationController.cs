using ElderCare_Domain.Commons;
using ElderCare_Repository;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            var result = await _unitOfWork.NotificationService.SendNotification(notificationModel);
            return Ok(result);
        }

        [Route("sendToAccount")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationToAccount(AccountNotiDto accountNotiDto)
        {
            var result = await _unitOfWork.NotificationService.SendNotificationToAccount(accountNotiDto);
            return Ok(result);
        }
    }
}
