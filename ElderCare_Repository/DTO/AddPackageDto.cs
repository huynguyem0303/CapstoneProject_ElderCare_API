using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddPackageDto
    {

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }
    }
}
