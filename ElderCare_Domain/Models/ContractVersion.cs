using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class ContractVersion
{
    public int ContractVersionId { get; set; }

    public int? ContractId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Status { get; set; }

    public virtual Contract Contract { get; set; }
}
