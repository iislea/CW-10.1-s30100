using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Edition
{
    public int EditionId { get; set; }

    public int? PublisherId { get; set; }

    public int? BookBookId { get; set; }

    public int? BookGenreId { get; set; }

    public int? BookAuthorId { get; set; }

    public string? BookTitle { get; set; }

    public string? Language { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Author? BookAuthor { get; set; }

    public virtual ICollection<Copy> Copies { get; set; } = new List<Copy>();

    public virtual Publisher? Publisher { get; set; }
}
