using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Email { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Accountstatus { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; } = new List<Rating>();
}
