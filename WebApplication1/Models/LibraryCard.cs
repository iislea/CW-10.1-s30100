using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class LibraryCard
{
    public int CardId { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public DateOnly? CreationDate { get; set; }

    public int? MemberId { get; set; }

    public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();

    public virtual LibraryMember? Member { get; set; }
}
