using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddReportDto
    {
        public int? CarerId { get; set; }

        public int? CustomerId { get; set; }

        public string? Description { get; set; }
    }
}
