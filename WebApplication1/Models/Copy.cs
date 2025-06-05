using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Copy
{
    public int CopyId { get; set; }

    public int? EditionId { get; set; }

    public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();

    public virtual Edition? Edition { get; set; }
}
