using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    //public int? CarerId { get; set; } //removed soon

    public Guid CarerServiceId { get; set; }

    public int? CustomerId { get; set; }

    public int? Ratng { get; set; }

    public string Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    //public virtual Carer Carer { get; set; } //removed soon

    public virtual Customer Customer { get; set; }

    public virtual CarerService CarerService { get; set; }
}
