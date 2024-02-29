using ElderCare_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class TrasactionDto
    {
        public double? FigureMoney { get; set; }
        public string? RedirectUrl { get; set; }
        public string? Type { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
