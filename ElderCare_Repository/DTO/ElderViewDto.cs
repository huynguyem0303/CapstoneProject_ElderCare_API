using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public partial class ElderViewDto
    {
        public int ElderlyId { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public string Relationshiptocustomer { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public string Note { get; set; }

        public int CustomerId { get; set; }

        public int LivingconditionId { get; set; }

        public LivingConditionDto? LivingCondition { get; set; }

        public class HobbyDto
        {
            public int HobbyId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public bool? Status { get; set; }
        }
        public List<HobbyDto> Hobbies { get; set; }
    }
}
