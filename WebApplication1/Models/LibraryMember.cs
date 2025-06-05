using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class LibraryMember
{
    public int MemberId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<LibraryCard> LibraryCards { get; set; } = new List<LibraryCard>();
}
