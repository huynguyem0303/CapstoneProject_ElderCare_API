using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? AccountId { get; set; }

    public double? FigureMoney { get; set; }

    public int? Type { get; set; }

    public string? Description { get; set; }

    public DateTime? Datetime { get; set; }

    public int? ContractId { get; set; }

    public int? CarercusId { get; set; }

    public string Status { get; set; }

    public virtual Account Account { get; set; } = new Account();

    public virtual Contract Contract { get; set; } = new Contract();
}
