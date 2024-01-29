using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Certification
{
    public int CertId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool? Status { get; set; }
}
