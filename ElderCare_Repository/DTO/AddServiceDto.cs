using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddServiceDto
    {

        public string? Name { get; set; }

        public string? Desciption { get; set; }

        public double? Price { get; set; }

        public int? Status { get; set; }
    }
}
