using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerPoint
{
    public int PointId { get; set; }

    public int? CarerId { get; set; }

    public double? Point { get; set; }
}
