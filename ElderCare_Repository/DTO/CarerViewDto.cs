using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class CarerViewDto
    {
        public int CarerId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public string? Age { get; set; }
        public string? Image { get; set; }
    }
}
