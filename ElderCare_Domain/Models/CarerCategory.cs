using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CarerCategory
{
    public int Carerid { get; set; }

    public int Cateid { get; set; }

    public virtual Carer Carer { get; set; }

    public virtual Category Cate { get; set; }
}
