using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Hobby
{
    public int HobbyId { get; set; }

    public int? ElderlyId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual Elderly? Elderly { get; set; }
}
