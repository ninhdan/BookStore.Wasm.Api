using System;
using System.Collections.Generic;

namespace BookStoreApi.Models;

public partial class Language
{
    public Guid LanguageId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string LanguageName { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
