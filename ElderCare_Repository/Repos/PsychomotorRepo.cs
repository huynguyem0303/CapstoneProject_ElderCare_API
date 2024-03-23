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
    }
}
