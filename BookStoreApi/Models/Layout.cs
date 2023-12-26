using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Layout
{
    public Guid LayoutId { get; set; }

    public string LayoutName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
