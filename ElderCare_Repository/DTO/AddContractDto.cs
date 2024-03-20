using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class AddContractDto
    {
        public int CarerId { get; set; }
        public int CustomerId { get; set; }
        public int ElderlyId { get; set; }
        public string[] service { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string Package { get; set; }
    }
}
