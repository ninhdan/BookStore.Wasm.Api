using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Rating
{
    public Guid RatingId { get; set; }

    public int RatingPoint { get; set; }

    public DateTime Time { get; set; }

    public string? Comment { get; set; }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
