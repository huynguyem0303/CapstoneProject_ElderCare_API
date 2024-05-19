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
    public interface IServicesService
    {
        IEnumerable<Service> GetAll();
        Task<Service?> GetById(int id);
        Task<IEnumerable<Service>> FindAsync(Expression<Func<Service, bool>> expression, params Expression<Func<Service, object>>[] includes);
        Task<Service> AddServiceAsync(AddServiceDto model);
        Task UpdateService(UpdateServiceDto model);
        Task DeleteService(int id);
        Task<bool> ServiceExists(int id);
        IEnumerable<Carer> GetCarerByServiceId(int serviceId);
        Task<TrackingOption> AddTrackingOption(AddTrackingOptionDto model);
        Task UpdateTrackingOption(UpdateTrackingOptionDto model);
        Task DeleteTrackingOption(int id);
        Task<bool> TrackingOptionExists(int id);
        Task<bool> ServiceNameExists(string name);
    }
}
