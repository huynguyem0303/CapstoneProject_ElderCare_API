using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public string? Desciption { get; set; }

    public double? Price { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<CarerService> CarerServices { get; set; } = new List<CarerService>();

    public virtual ICollection<ContractService> ContractServices { get; set; } = new List<ContractService>();
}
