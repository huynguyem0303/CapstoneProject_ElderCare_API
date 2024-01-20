using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Bankinformation
{
    public int BankinfoId { get; set; }

    public int? AccountNumber { get; set; }

    public string BankName { get; set; }

    public string Branch { get; set; }

    public string AccountName { get; set; }

    public virtual ICollection<Carer> Carers { get; set; } = new List<Carer>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
