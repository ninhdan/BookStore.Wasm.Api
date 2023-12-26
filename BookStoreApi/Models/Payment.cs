using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public string Paymentmethod { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime Paymentdate { get; set; }

    public string Paymentstatus { get; set; } = null!;

    public Guid OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
