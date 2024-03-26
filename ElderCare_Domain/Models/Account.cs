using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Account
{
    public int RoleId { get; set; }

    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int? Status { get; set; }

    public int? CustomerId { get; set; }

    public int? CarerId { get; set; }
    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
