using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class UpdateServiceDto
    {
        public int ServiceId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public int? Status { get; set; }
    }
}
