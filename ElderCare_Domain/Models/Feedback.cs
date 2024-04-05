using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? CustomerId { get; set; }

    public int? Ratng { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CarerServiceId { get; set; }

    public virtual CarerService? CarerService { get; set; }

    public virtual Customer? Customer { get; set; }
}
