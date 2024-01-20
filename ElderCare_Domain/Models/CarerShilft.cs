using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerShilft
{
    public int? CarerId { get; set; }

    public int? ShilfId { get; set; }

    public virtual Carer Carer { get; set; }

    public virtual Shilft Shilf { get; set; }
}
