using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Package
{
    public int PackageId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<PackageService> PackageServices { get; set; } = new List<PackageService>();
}
