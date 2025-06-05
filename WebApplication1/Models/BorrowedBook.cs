using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class BorrowedBook
{
    public int BorrowId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public int? CopyId { get; set; }

    public int? StaffId { get; set; }

    public int? ManagerId { get; set; }

    public int? CardId { get; set; }

    public virtual LibraryCard? Card { get; set; }

    public virtual Copy? Copy { get; set; }
}
