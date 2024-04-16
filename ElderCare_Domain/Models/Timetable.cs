using ElderCare_Domain.Enums;
using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Timetable
{
    public int TimetableId { get; set; }

    public DateTime? ReportDate { get; set; }

    public string? Timeframe { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public int? Status { get; set; } = (int?)TimetableStatus.Pending;

    public int? CarerId { get; set; }

    public int? ContractId { get; set; }

    public virtual ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();
}
