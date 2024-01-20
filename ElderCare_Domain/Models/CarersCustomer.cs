using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarersCustomer
{
    public int? CustomerId { get; set; }

    public int? CarerId { get; set; }

    public DateTime? Datetime { get; set; }

    public int CarercusId { get; set; }

    public virtual Carer Carer { get; set; }

    public virtual Customer Customer { get; set; }
}
