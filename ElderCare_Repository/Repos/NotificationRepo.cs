using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        public NotificationRepo(ElderCareContext context) : base(context)
        {
        }

        public IEnumerable<Notification> GetAllByCustomerId(int customerId)
        {
            var list = from noti in _dbSet
                       join acc in _context.Accounts.Where(e => e.CustomerId == customerId) on noti.AccountId equals acc.AccountId
                       select noti;
            return list;
        }
        public IEnumerable<Notification> GetAllByCarerId(int carerId)
        {
            var list = from noti in _dbSet
                       join acc in _context.Accounts.Where(e => e.CarerId == carerId) on noti.AccountId equals acc.AccountId
                       select noti;
            return list;
        }
    }
}
