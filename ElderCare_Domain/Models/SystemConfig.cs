using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class SystemConfig
{
    public int? SystemConfigId { get; set; }
    public string DataName { get; set; }
    public string DataValue { get; set; }
}
