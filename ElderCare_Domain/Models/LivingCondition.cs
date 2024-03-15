using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class LivingCondition
{
    public int LivingconId { get; set; }

    public bool? LiveWithRelative { get; set; }

    public string? Regions { get; set; }

    public bool? HaveSeperateRoom { get; set; }

    public string? Others { get; set; }

    public virtual ICollection<Elderly> Elderlies { get; set; } = new List<Elderly>();
}
