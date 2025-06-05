using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

    public virtual ICollection<Edition> Editions { get; set; } = new List<Edition>();
}
