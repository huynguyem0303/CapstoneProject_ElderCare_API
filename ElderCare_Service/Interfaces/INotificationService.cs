using ElderCare_Domain.Commons;
using ElderCare_Repository.DTO;
namespace ElderCare_Service.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
        Task<ResponseModel> SendNotificationToAccount(AccountNotiDto accountNotiDt);
    }
}
