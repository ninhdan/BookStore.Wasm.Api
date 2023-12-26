using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class PriceRange
{
    public Guid RangeId { get; set; }

    public string RangeName { get; set; } = null!;

    public decimal MinPrice { get; set; }

    public decimal MaxPrice { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
