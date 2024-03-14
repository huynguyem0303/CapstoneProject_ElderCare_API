﻿using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
