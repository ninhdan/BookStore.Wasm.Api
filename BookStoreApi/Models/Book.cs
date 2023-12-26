using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Book
{
    public Guid BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public DateTime PublisherYear { get; set; }

    public string Size { get; set; } = null!;

    public string? Translator { get; set; }

    public int Weight { get; set; }

    public int QuanlityPage { get; set; }

    public string? Description { get; set; }

    public Guid SupplierId { get; set; }

    public Guid LanguageId { get; set; }

    public Guid SubcategoryId { get; set; }

    public Guid LayoutId { get; set; }

    public Guid PriceRangeId { get; set; }

    public decimal Price { get; set; }

    public string Author { get; set; } = null!;

    public string PathImage { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual Language Language { get; set; } = null!;

    public virtual Layout Layout { get; set; } = null!;

    public virtual PriceRange PriceRange { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; } = new List<Rating>();

    public virtual SubCategory Subcategory { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
