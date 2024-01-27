using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Account
{
    public int RoleId { get; set; }

    public int AccountId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string? Address { get; set; }

    public bool? Status { get; set; }

    public int? CustomerId { get; set; }

    public int? CarerId { get; set; }
}
