using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerShilft
{
    public int? CarerId { get; set; }

    public int? ShilftId { get; set; }

    public virtual Carer? Carer { get; set; } = new Carer();

    public virtual Shilft? Shilft { get; set; } = new Shilft();
}
