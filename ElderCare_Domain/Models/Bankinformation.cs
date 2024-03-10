using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Bankinformation
{
    public int BankinfoId { get; set; }

    public string BankAccountNumber { get; set; }

    public string BankName { get; set; }

    public string Branch { get; set; }

    public string BankAccountName { get; set; }

    public virtual ICollection<Carer> Carers { get; set; } = new List<Carer>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    //public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
