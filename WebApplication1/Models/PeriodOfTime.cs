using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class PeriodOfTime
{
    public string NameOfPeriod { get; set; } = null!;

    public int? SinceYear { get; set; }

    public int? TillYear { get; set; }
}
