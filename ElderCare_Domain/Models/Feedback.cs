using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? CustomerId { get; set; }

    public int? Ratng { get; set; }

    public string? Description { get; set; }

    public Guid? CarerServiceId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Customer? Customer { get; set; }
}
