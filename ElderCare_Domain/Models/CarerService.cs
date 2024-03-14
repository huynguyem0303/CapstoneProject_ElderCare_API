using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerService
{
   

    public int? CarerId { get; set; }

    public int? ServiceId { get; set; }

    public int CarerServiceId { get; set; }

    public virtual Carer? Carer { get; set; } 
    public virtual Service? Service { get; set; } 

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

   
}
