using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Book
{
    public int BookId { get; set; }

    public int GenreId { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

    public virtual ICollection<Edition> Editions { get; set; } = new List<Edition>();

    public virtual Genre Genre { get; set; } = null!;
}
