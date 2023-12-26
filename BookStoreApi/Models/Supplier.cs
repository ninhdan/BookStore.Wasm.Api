using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Supplier
{
    public Guid SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
