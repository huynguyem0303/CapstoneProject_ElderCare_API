using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class ContractService
{
    public int? ContractId { get; set; }

    public int? ServiceId { get; set; }

    public double? Price { get; set; }

    public int ContractServicesId { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual Service? Service { get; set; }

    public virtual ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();
}
