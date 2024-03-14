using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Fcmtoken
{
    public Guid TokenId { get; set; }

    public int AccountId { get; set; }

    public string TokenDescription { get; set; }

    public virtual Account Account { get; set; }
}
