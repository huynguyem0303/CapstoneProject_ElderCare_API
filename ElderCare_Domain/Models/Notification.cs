using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Notification
{
    public Guid NotiId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = new Account();
}
