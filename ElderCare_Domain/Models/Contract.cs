using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? CarerId { get; set; }

    public int? CustomerId { get; set; }

    public int? ElderlyId { get; set; }

    public int? Status { get; set; }

    public int? ContractType { get; set; }

    public double? Packageprice { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? PackageId { get; set; }

    public virtual Carer Carer { get; set; }

    public virtual ICollection<ContractService> ContractServices { get; set; } = new List<ContractService>();

    public virtual ICollection<ContractVersion> ContractVersions { get; set; } = new List<ContractVersion>();

    public virtual Customer Customer { get; set; } = new Customer();

    public virtual Elderly Elderly { get; set; } = new Elderly();

    public virtual Package Package { get; set; } = new Package();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
