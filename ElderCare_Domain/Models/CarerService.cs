using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerService
{
    public Guid CarerServiceId { get; set; }

    public int? CarerId { get; set; }

    public int? ServiceId { get; set; }

    public virtual Carer? Carer { get; set; }

    public virtual Service? Service { get; set; }
}
