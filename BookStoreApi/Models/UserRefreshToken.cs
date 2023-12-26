using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class UserRefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime ExpirationDate { get; set; }
}
