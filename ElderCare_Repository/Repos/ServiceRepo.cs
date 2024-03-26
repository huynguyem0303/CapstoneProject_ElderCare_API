using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ElderCare_Repository.Repos
{
    public class ServiceRepo : GenericRepo<Service>, IServiceRepo
    {
        public ServiceRepo(ElderCareContext context) : base(context)
        {
        }

        public async Task<List<Service>> GetAllByCarerId(int id)
        {
            var list = _context.CarerServices.Where(e => e.CarerId == id);
            var result = from service in _dbSet
                         join carerService in list on service.ServiceId equals carerService.ServiceId
                         select service;
            return await result.ToListAsync();
        }
    }
}
