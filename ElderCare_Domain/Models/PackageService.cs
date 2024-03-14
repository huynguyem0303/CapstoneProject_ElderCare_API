using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class PackageService
{
    public int? ServiceId { get; set; }

    public int? PackageId { get; set; }

    public int PackageServicesId { get; set; }

    public virtual Package Package { get; set; }

    public virtual ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();
}
