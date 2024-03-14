using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface IHealthDetailRepo : IGenericRepo<HealthDetail>
    {
        Task AddHealthDetail(int elderlyId, HealthDetail healthDetail);
    }
}
