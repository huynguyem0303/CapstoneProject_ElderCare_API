using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddElderDto
    {
        public string Name { get; set; }

        public int? Age { get; set; }

        public string Relationshiptocustomer { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public string Note { get; set; }

        public int CustomerId { get; set; }
    }
}
