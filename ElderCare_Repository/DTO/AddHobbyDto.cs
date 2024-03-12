using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public partial class AddElderDto
    {
        public class AddHobbyDto
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public bool? Status { get; set; }
        }
        public List<AddHobbyDto>? Hobbies { get; set; }
    }
}
