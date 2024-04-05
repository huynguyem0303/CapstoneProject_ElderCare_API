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
    public interface ICarerService
    {
        IEnumerable<Carer> GetAll();
        Task<Carer?> GetById(int id);
        Task<List<CarerViewDto>> GetByPending();
        Task<IEnumerable<Category>> FindCateAsync(Expression<Func<Category, bool>> expression, params Expression<Func<Category, object>>[] includes);
        Task<IEnumerable<Carer>> FindAsync(Expression<Func<Carer, bool>> expression, params Expression<Func<Carer, object>>[] includes);
        //Task<Carer> AddCarerAsync(Carer model);
        Task UpdateCarer(Carer Carer);
        Task DeleteCarer(int id);
        Task<bool> CarerExists(int id);
        Task<List<CarerViewDto>?> SearchCarer(SearchCarerDto dto);
       
        Task<List<TransactionHistoryDto>> GetTransactionHistoryByCarerIdAsync(int carerId);
        Task<List<TransactionHistoryDto>> GetTransactionHistoryByCustomerIdAsync(int customerId);
        Task<Account?> ApproveCarer(int carerId, int status);
        Task<List<ServiceDto>> GetServicesByCarerId(int id);
        Task<List<CarerServiceDto>> AddCarerServiceAsync(int carerId, string[] serviceName);
        Task RemoveCarerService(int carerId, int serviceId);
        Task<List<FeedbackDto>> FindCarerFeedback(Expression<Func<Feedback, bool>> expression);
        Task<FeedbackDto> AddServiceFeedback(AddFeedbackDto model);
        Task UpdateCarerServiceFeedback(UpdateFeedbackDto model);
        Task RemoveCarerServiceFeedback(int feedbackId);
        Task<bool> FeedbackExist(int carerId, int serviceId, int feedbackId);
    }
}
