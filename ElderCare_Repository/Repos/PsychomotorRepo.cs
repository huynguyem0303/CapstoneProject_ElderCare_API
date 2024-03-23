using DataAccess.Repositories;
using ElderCare_Domain.Models;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Repos
{
    public class PsychomotorRepo : GenericRepo<Psychomotor>, IPsychomotorRepo
    {
        public PsychomotorRepo(ElderCareContext context) : base(context)
        {
        }

        public new async Task AddAsync(Psychomotor entity)
        {
            try
            {
                entity.PsychomotorHealthId = _dbSet.OrderBy(e => e.PsychomotorHealthId).Last().PsychomotorHealthId + 1;
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
    }
}
