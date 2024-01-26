using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Category
{
    public int CateId { get; set; }

    public int? CateType { get; set; }

    public string Desciption { get; set; }
}
