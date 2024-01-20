using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? CarerId { get; set; }

    public int? CustomerId { get; set; }

    public int? Ratng { get; set; }

    public string Description { get; set; }

    public virtual Carer Carer { get; set; }

    public virtual Customer Customer { get; set; }
}
