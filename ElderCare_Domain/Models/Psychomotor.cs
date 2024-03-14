using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Psychomotor
{
    public int PsychomotorHealthId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<PsychomotorHealth> PsychomotorHealths { get; set; } = new List<PsychomotorHealth>();
}
