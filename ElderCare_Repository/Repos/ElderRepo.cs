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
    public class ElderRepo : GenericRepo<Elderly>, IElderRepo
    {
        public ElderRepo(ElderCareContext context) : base(context)
        {

        }
        public new IEnumerable<Elderly> GetAll()
        {
            //CheckIfNullException();
            return _dbSet;
        }

        public new void Update(Elderly entity)
        {
            try
            {
                if (!entity.LivingconditionId.HasValue)
                {
                    entity.LivingconditionId = _context.LivingConditions.OrderBy(e => e.LivingconId).Select(e => e.LivingconId).Last()+1;
                    entity.Livingcondition.LivingconId = (int)entity.LivingconditionId;
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CheckIfNullException()
        {
            _ = _dbSet.Select(e => e.ElderlyId).ToList();
            _ = _dbSet.Select(e => e.HealthDetailId).ToList();
            _ = _dbSet.Select(e => e.LivingconditionId).ToList();
            _ = _dbSet.Select(e => e.Name).ToList();
            _ = _dbSet.Select(e => e.Age).ToList();
            _ = _dbSet.Select(e => e.Relationshiptocustomer).ToList();
            _ = _dbSet.Select(e => e.Address).ToList();
            _ = _dbSet.Select(e => e.Image).ToList();
            _ = _dbSet.Select(e => e.Note).ToList();
            _ = _dbSet.Select(e => e.CustomerId).ToList();
            //_ = _dbSet.Select(e => e.HobbyId).ToList();

        }
    }
}
