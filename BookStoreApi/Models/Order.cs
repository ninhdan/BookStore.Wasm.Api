using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Order
{
    public Guid OrderId { get; set; }

    public DateTime? DateOrder { get; set; }

    public string? Note { get; set; }

    public decimal? Grandtotal { get; set; }

    public decimal? Feeshipp { get; set; }

    public Guid UserId { get; set; }

    public bool Issubmitted { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
