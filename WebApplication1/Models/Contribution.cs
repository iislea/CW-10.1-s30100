using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Contribution
{
    public int AuthorId { get; set; }

    public int BookBookId { get; set; }

    public int BookGenreId { get; set; }

    public string? BookTitle { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}
