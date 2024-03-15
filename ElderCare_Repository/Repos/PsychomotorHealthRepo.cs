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
    public class PsychomotorHealthRepo : GenericRepo<PsychomotorHealth>, IPsychomotorHealthRepo
    {
        public PsychomotorHealthRepo(ElderCareContext context) : base(context)
        {
        }

        public async Task<PsychomotorHealth?> GetByIdsAsync(int healthDetailId, int psychomotorHealthId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.HealthDetailId == healthDetailId && e.PsychomotorHealthId == psychomotorHealthId);
        }
    }
}
