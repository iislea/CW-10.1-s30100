using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class LibraryStaff
{
    public int StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? ManagerId { get; set; }

    public virtual ICollection<LibraryStaff> InverseManager { get; set; } = new List<LibraryStaff>();

    public virtual LibraryStaff? Manager { get; set; }
}
