using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerCategory
{
    public int? CateId { get; set; }

    public int? CarerId { get; set; }

    public virtual Carer Carer { get; set; }

    public virtual Category Cate { get; set; }
}
