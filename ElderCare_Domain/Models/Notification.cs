using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Notification
{
    public Guid NotiId { get; set; }

    public string? Title { get; set; }

    public string? SubTitle { get; set; }

    public string? Sound { get; set; }

    public int Badge { get; set; }

    public string ChannelId { get; set; }

    public string? Body { get; set; }

    public bool MutableContent { get; set; }

    public int AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
