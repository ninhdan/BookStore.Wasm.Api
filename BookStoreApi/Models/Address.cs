using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Address
{
    public Guid AddressId { get; set; }

    public string StreetNumber { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string Country { get; set; } = null!;

    public Guid UserId { get; set; }

    public bool StatusAddress { get; set; }

    public virtual User User { get; set; } = null!;
}
