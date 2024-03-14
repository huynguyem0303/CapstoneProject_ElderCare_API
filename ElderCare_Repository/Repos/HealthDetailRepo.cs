using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class HealthDetailRepo : GenericRepo<HealthDetail>, IHealthDetailRepo
    {
        public HealthDetailRepo(ElderCareContext context) : base(context)
        {
        }


        public async Task AddHealthDetail(int elderlyId, HealthDetail healthDetail)
        {
            try
            {
                var elderly = _context.Elderlies.Find(elderlyId) ?? throw new NullReferenceException("elderly id is invalid"); 
                if (elderly.HealthDetailId != null)
                {
                    throw new Exception("Elder already has health detail");
                }
                elderly.HealthDetailId = healthDetail.HealthDetailId;
                elderly.HealthDetail = healthDetail;
                _context.Elderlies.Update(elderly);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
