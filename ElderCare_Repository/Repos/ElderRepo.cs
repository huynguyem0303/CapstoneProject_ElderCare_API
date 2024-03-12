using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public new async Task AddAsync(Elderly entity)
        {
            try
            {
                if (entity.Livingcondition != null)
                {
                    entity.LivingconditionId = _context.LivingConditions.OrderBy(e => e.LivingconId).Select(e => e.LivingconId).Last() + 1;
                    entity.Livingcondition.LivingconId = (int)entity.LivingconditionId;
                }
                if (!entity.Hobbies.IsNullOrEmpty())
                {
                    int lastId = _context.Hobbies.OrderBy(e => e.HobbyId).Select(e => e.HobbyId).Last()+1;
                    foreach (var hobby in entity.Hobbies)
                    {
                        hobby.HobbyId = lastId++;
                    }
                }
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public new void Delete(Elderly entity)
        {
            try
            {
                _dbSet.Remove(entity);
                var livingCondition = _context.LivingConditions.FirstOrDefault(e => e.LivingconId == entity.LivingconditionId);
                if (livingCondition != null)
                {
                    _context.LivingConditions.Remove(livingCondition);
                }
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
