using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Elderly
{
    public int ElderlyId { get; set; }

    public int? HealthDetailId { get; set; }

    public int? LivingconditionId { get; set; }

    public string Name { get; set; }

    public int? Age { get; set; }

    public string Relationshiptocustomer { get; set; }

    public string Address { get; set; }

    public string Image { get; set; }

    public string Note { get; set; }

    public int? CustomerId { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Customer Customer { get; set; }

    public virtual HealthDetail HealthDetail { get; set; }

    public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();

    public virtual LivingCondition Livingcondition { get; set; }
}
