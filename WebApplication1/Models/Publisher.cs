using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? PublisherName { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Edition> Editions { get; set; } = new List<Edition>();
}
