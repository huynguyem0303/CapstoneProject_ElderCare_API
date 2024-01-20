using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class HealthDetail
{
    public int HealthDetailId { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public string MedicalCondition { get; set; }

    public string BloodPressure { get; set; }

    public string HeartProblems { get; set; }

    public string DiabetesType { get; set; }

    public string StomachAche { get; set; }

    public string VestibularDisorders { get; set; }

    public string Allergy { get; set; }

    public virtual ICollection<Elderly> Elderlies { get; set; } = new List<Elderly>();
}
