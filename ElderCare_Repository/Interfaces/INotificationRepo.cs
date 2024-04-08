using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface INotificationRepo : IGenericRepo<Notification>
    {
        IEnumerable<Notification> GetAllByCustomerId(int customerId);
        IEnumerable<Notification> GetAllByCarerId(int carerId);
    }
}
