using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class PsychomotorHealth
{
    public Guid PsychomotorHealthDetailId { get; set; }

    public int? HealthDetailId { get; set; }

    public int? PsychomotorHealthId { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual HealthDetail? HealthDetail { get; set; }

    public virtual Psychomotor? PsychomotorHealthNavigation { get; set; }
}
