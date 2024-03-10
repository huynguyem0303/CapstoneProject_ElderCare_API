using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Domain.Models
{
    public class CarerService
    {
        public Guid CarerServiceId { get; set; }

        public int ServiceId { get; set; }

        public int CarerId { get; set; }

        public virtual Carer Carer {get; set;}

        public virtual Service Service { get; set;}

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
