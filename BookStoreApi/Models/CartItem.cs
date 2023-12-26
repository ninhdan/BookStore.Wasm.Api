using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class CartItem
{
    public Guid ItemId { get; set; }

    public int Quanlity { get; set; }

    public Guid BookId { get; set; }

    public Guid OrderId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
