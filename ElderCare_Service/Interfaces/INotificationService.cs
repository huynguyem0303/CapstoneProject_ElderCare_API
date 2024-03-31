using ElderCare_Domain.Commons;
using ElderCare_Repository.DTO;
using ExpoCommunityNotificationServer.Models;
namespace ElderCare_Service.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
        Task<ResponseModel> SendNotificationToAccount(AccountNotiDto accountNotiDt);
        Task<PushTicketResponse> SendExpoNotificationToAccount(AccountExpoNotiDto accountNotiModel);
        Task<PushTicketResponse> SendExpoNotification(PushTicketRequestDto[] pushTicketReq);
        Task<PushReceiptResponse> GetExpoNotificationReceipt(PushReceiptRequest pushReceiptReq);
    }
}
