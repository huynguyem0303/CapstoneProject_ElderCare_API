using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Timetable
{
    public int TimetableId { get; set; }

    public DateTime? ReportDate { get; set; }

    public string? Timeframe { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? Status { get; set; }

    public int? CarerId { get; set; }

    public virtual ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();
}
