using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class PackageDto
    {
        public int PackageId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public virtual ICollection<ServiceDto> PackageServices { get; set; }
    }
}
