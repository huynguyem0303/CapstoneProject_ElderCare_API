using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Timetable
{
    public int TimetableId { get; set; }

    public int? PackageServicesId { get; set; }

    public int? ContractServicesId { get; set; }

    public DateTime? ReportDate { get; set; }

    public string Timeframe { get; set; }

    public string Image { get; set; }

    public string ReportContent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Status { get; set; }

    public bool? CusApprove { get; set; }

    public string CusContent { get; set; }

    public int? CarerId { get; set; }

    public virtual ContractService ContractServices { get; set; }

    public virtual PackageService PackageServices { get; set; }
}
