using DataAccess.Interfaces;
using DataAccess.Repositories;
using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface IServiceRepo : IGenericRepo<Service>
    {
        Task<List<Service>> GetAllByCarerId(int id);
    }
}
