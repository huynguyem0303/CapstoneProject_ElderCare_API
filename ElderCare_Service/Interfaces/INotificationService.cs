using ElderCare_Domain.Commons;
using ElderCare_Repository.DTO;
using Expo.Server.Models;
namespace ElderCare_Service.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
        Task<ResponseModel> SendNotificationToAccount(AccountNotiDto accountNotiDt);
        Task<PushTicketResponse> SendExpoNotification(PushTicketRequest pushTicketReq);
        Task<PushResceiptResponse> GetExpoNotificationReceipt(PushReceiptRequest pushReceiptReq);
    }
}
