using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ElderCare_Domain.Models;

public partial class Account
{
    public int RoleId { get; set; }

    public int AccountId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public int? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public bool? Status { get; set; }

    public int? CustomerId { get; set; }

    public int? CarerId { get; set; }

    public virtual Carer? Carer { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Role? Role { get; set; }
}
