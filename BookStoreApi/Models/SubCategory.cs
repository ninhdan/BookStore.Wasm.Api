using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class SubCategory
{
    public Guid SubcategoryId { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();

    public virtual Category Category { get; set; } = null!;
}
