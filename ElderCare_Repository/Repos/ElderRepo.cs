using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
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
