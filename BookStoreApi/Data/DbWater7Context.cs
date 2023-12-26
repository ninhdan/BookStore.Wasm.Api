using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Models;

public partial class DbWater7Context : DbContext
{
    public DbWater7Context()
    {
    }

    public DbWater7Context(DbContextOptions<DbWater7Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Layout> Layouts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PriceRange> PriceRanges { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=DB_Water7;Trusted_Connection=True;MultipleActiveResultSets=True; TrustServerCertificate=True; encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK_Address_address_id");

            entity.ToTable("Address");

            entity.Property(e => e.AddressId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("address_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Province)
                .HasMaxLength(100)
                .HasColumnName("province");
            entity.Property(e => e.StatusAddress).HasColumnName("status_address");
            entity.Property(e => e.StreetNumber)
                .HasMaxLength(100)
                .HasColumnName("street_number");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Ward)
                .HasMaxLength(100)
                .HasColumnName("ward");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK_Book_book_id");

            entity.ToTable("Book");

            entity.Property(e => e.BookId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("book_id");
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .HasColumnName("author");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.LayoutId).HasColumnName("layout_id");
            entity.Property(e => e.PathImage)
                .IsUnicode(false)
                .HasColumnName("path_image");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.PriceRangeId).HasColumnName("price_range_id");
            entity.Property(e => e.Publisher)
                .HasMaxLength(100)
                .HasColumnName("publisher");
            entity.Property(e => e.PublisherYear)
                .HasColumnType("date")
                .HasColumnName("publisher_year");
            entity.Property(e => e.QuanlityPage).HasColumnName("quanlity_page");
            entity.Property(e => e.Size)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("size ");
            entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Translator)
                .HasMaxLength(100)
                .HasColumnName("translator");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Layout).WithMany(p => p.Books)
                .HasForeignKey(d => d.LayoutId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PriceRange).WithMany(p => p.Books)
                .HasForeignKey(d => d.PriceRangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_PriceRange_range_id");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Books)
                .HasForeignKey(d => d.SubcategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Books)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK_Cart_Item_item_id");

            entity.ToTable("Cart_Item", tb => tb.HasTrigger("tr_CartItemChanged"));

            entity.Property(e => e.ItemId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("item_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quanlity).HasColumnName("quanlity");

            entity.HasOne(d => d.Book).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Order).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Category_category_id");

            entity.ToTable("Category");

            entity.HasIndex(e => e.CategoryName, "UQ__Category__5189E25519904C57").IsUnique();

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
            entity.Property(e => e.ProductPortfolio).HasColumnName("product_portfolio");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK_Language_language_id");

            entity.ToTable("Language");

            entity.HasIndex(e => e.LanguageCode, "UQ__Language__A6D3AFDB84E98E65").IsUnique();

            entity.Property(e => e.LanguageId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("language_id");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("language_code");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(50)
                .HasColumnName("language_name");
        });

        modelBuilder.Entity<Layout>(entity =>
        {
            entity.HasKey(e => e.LayoutId).HasName("PK_Layout_layout_id");

            entity.ToTable("Layout");

            entity.Property(e => e.LayoutId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("layout_id");
            entity.Property(e => e.LayoutName)
                .HasMaxLength(50)
                .HasColumnName("layout_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Order_order_id");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("order_id");
            entity.Property(e => e.DateOrder)
                .HasColumnType("datetime")
                .HasColumnName("date_order");
            entity.Property(e => e.Feeshipp)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("feeshipp");
            entity.Property(e => e.Grandtotal)
                .HasColumnType("money")
                .HasColumnName("grandtotal");
            entity.Property(e => e.Issubmitted).HasColumnName("issubmitted");
            entity.Property(e => e.Note)
                .HasColumnType("text")
                .HasColumnName("note");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EA2ECA318B");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Paymentdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("paymentdate");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(100)
                .HasColumnName("paymentstatus");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PriceRange>(entity =>
        {
            entity.HasKey(e => e.RangeId).HasName("PK_PriceRange_pricerange_id");

            entity.ToTable("PriceRange");

            entity.HasIndex(e => e.RangeName, "UQ__PriceRan__565EA2675049B906").IsUnique();

            entity.Property(e => e.RangeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("range_id");
            entity.Property(e => e.MaxPrice)
                .HasColumnType("money")
                .HasColumnName("max_price ");
            entity.Property(e => e.MinPrice)
                .HasColumnType("money")
                .HasColumnName("min_price ");
            entity.Property(e => e.RangeName)
                .HasMaxLength(50)
                .HasColumnName("range_name");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK_Rating_rating_id");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("rating_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.RatingPoint).HasColumnName("rating_point");
            entity.Property(e => e.Time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("time");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubcategoryId).HasName("PK_BookType_booktype_id");

            entity.ToTable("SubCategory");

            entity.HasIndex(e => e.SubcategoryName, "UQ__SubCateg__5B73707365554083").IsUnique();

            entity.Property(e => e.SubcategoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("subcategory_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.SubcategoryName)
                .HasMaxLength(100)
                .HasColumnName("subcategory_name");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK_Supplier_supplier_id");

            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("supplier_id");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(50)
                .HasColumnName("supplier_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Customer_customer_id");

            entity.ToTable("User");

            entity.HasIndex(e => e.Phone, "UniquePhone").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_id");
            entity.Property(e => e.Accountstatus).HasColumnName("accountstatus");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(200)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserRefreshToken_Id");

            entity.ToTable("UserRefreshToken");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
