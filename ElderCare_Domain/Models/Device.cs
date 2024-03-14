using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Device
{
    public Guid DeviceId { get; set; }

    public int AccountId { get; set; }

    public string DeviceFcmToken { get; set; }

    public virtual Account Account { get; set; }
}
