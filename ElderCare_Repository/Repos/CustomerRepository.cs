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
    public class CustomerRepository : GenericRepo<Customer>, ICustomerRepository
    {
        public CustomerRepository(ElderCareContext context) : base(context)
        {

        }
        

    }
}
