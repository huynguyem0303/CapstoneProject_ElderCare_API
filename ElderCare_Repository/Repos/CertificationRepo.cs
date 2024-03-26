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
    public class CertificationRepo : GenericRepo<Certification>, ICertificationRepo
    {
        public CertificationRepo(ElderCareContext context) : base(context)
        {
        }
    }
}
