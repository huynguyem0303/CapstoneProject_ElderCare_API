﻿using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class CertificationCarer
{
    public Guid CarerCertId { get; set; }

    public int? CertId { get; set; }

    public int? CarerId { get; set; }

    public string? Qualificationurl { get; set; }

    public virtual Carer? Carer { get; set; }

    public virtual Certification? Cert { get; set; }
}
