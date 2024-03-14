using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int? CarerId { get; set; }

    public int? CustomerId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Carer? Carer { get; set; }

    public virtual Customer? Customer { get; set; }
}
