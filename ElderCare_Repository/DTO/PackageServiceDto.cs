using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class PackageServiceDto
    {
        public int PackageServicesId { get; set; }

        public int? PackageId { get; set; }

        public int? ServiceId { get; set; }

        public string? ServiceName { get; set; }

    }
}
