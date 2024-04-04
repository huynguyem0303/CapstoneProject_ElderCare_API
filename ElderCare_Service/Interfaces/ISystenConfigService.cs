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
    public interface ISystemConfigService
    {
        IEnumerable<SystemConfig> GetAll();
        Task<SystemConfig?> GetById(int id);
        Task<IEnumerable<SystemConfig>> FindAsync(Expression<Func<SystemConfig, bool>> expression, params Expression<Func<SystemConfig, object>>[] includes);
        Task<SystemConfig> AddSystemConfigAsync(AddSystemConfigDto model);
        Task UpdateSystemConfig(SystemConfig model);
        Task DeleteSystemConfig(int id);
        Task<bool> SystemConfigExists(int id);
    }
}
