using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class LivingConditionDto
    {
        public bool? LiveWithRelative { get; set; }

        public string Regions { get; set; }

        public bool? HaveSeperateRoom { get; set; }

        public string Others { get; set; }
    }
    public partial class AddElderDto
    {
        public LivingConditionDto? LivingCondition { get; set; }
    }
}
