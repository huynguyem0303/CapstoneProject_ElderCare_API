using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class SearchCarerDto
    {
        public string ServiceDes { get; set; }
        public string[] TimeShift { get; set; }
        public string[] Gender { get; set; }
        public string[] Age { get; set; }
        public string[] District { get; set; }
        public string[] Cate { get; set; }
    }
}
