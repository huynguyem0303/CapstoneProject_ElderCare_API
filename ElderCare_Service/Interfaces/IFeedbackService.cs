using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface IFeedbackService
    {
        IEnumerable<Feedback> GetAll();
        Task<IEnumerable<Feedback>> FindAsync(Expression<Func<Feedback, bool>> expression, params Expression<Func<Feedback, object>>[] includes);
        Task DeleteFeedback(int id);
        Task<bool> FeedbackExists(int id);
    }
}
