using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MilkShop.Data.Models;

public partial class MilkShopContext : DbContext
{
    public MilkShopContext()
    {
    }

    public MilkShopContext(DbContextOptions<MilkShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBrand> ProductBrands { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =120.72.85.82,9033; database = MilkShop;uid=sa;pwd=f0^wyhMfl*25;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.CompanyAddress)
                .HasMaxLength(50)
                .HasColumnName("companyAddress");
            entity.Property(e => e.CompanyEmail)
                .HasMaxLength(50)
                .HasColumnName("companyEmail");
            entity.Property(e => e.CompanyFirstName)
                .HasMaxLength(50)
                .HasColumnName("companyFirstName");
            entity.Property(e => e.CompanyLastName)
                .HasMaxLength(50)
                .HasColumnName("companyLastName");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .HasColumnName("companyName");
            entity.Property(e => e.CompanyPhone)
                .HasMaxLength(50)
                .HasColumnName("companyPhone");
            entity.Property(e => e.CreateDated)
                .HasColumnType("datetime")
                .HasColumnName("create_dated");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("status");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CFFD1B40A8E");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserEmail, "UQ__Users__D54ADF550471942A").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
            entity.Property(e => e.PreferredLanguage)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("preferred_language");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .HasColumnName("userEmail");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");
            entity.Property(e => e.ImageUrl)
               .HasColumnType("nvarchar(max)")
               .HasColumnName("image_url");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809335DF29DA3F0");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.BillingAddress)
                .HasMaxLength(255)
                .HasColumnName("billingAddress");
            entity.Property(e => e.CreatedDate)
                .HasMaxLength(50)
                .HasColumnName("created_date");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasColumnName("orderStatus");
            entity.Property(e => e.OrderTotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("orderTotalAmount");
            entity.Property(e => e.PaymentMethodId)
                .HasMaxLength(50)
                .HasColumnName("paymentMethodId");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");
            entity.Property(e => e.ShippingAddress)
                .HasMaxLength(255)
                .HasColumnName("shippingAddress");
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(50)
                .HasColumnName("shippingMethod");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__userId__33D4B598");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE4A4D5D4DC7");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailId");
            entity.Property(e => e.CreateDate).HasColumnName("create_date");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.OrderdetailPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("orderdetail_price");
            entity.Property(e => e.ProductDiscount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("productDiscount");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("productPrice");
            entity.Property(e => e.ProductQuantity).HasColumnName("productQuantity");
            entity.Property(e => e.ProductWeight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("productWeight");
            entity.Property(e => e.ShippingCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("shippingCost");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TaxAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("taxAmount");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__order__37A5467C");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__produ__38996AB5");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__2D10D16A3FD5F932");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.ProductBrandId).HasColumnName("productBrandId");
            entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");
            entity.Property(e => e.ProductDescription).HasColumnName("productDescription");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .HasColumnName("productImage");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("productPrice");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.StockQuantity).HasColumnName("stockQuantity");

            entity.HasOne(d => d.ProductBrand).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductBrandId)
                .HasConstraintName("FK__Products__produc__29572725");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("Fk_pro_ca");
        });

        modelBuilder.Entity<ProductBrand>(entity =>
        {
            entity.HasKey(e => e.ProductBrandId).HasName("PK__ProductB__FF6FDF7849944520");

            entity.Property(e => e.ProductBrandId).HasColumnName("productBrandId");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(50)
                .HasColumnName("contact_email");
            entity.Property(e => e.CountryOfOrigin)
                .HasMaxLength(50)
                .HasColumnName("country_of_origin");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(50)
                .HasColumnName("logo_url");
            entity.Property(e => e.ProductBrandName)
                .HasMaxLength(255)
                .HasColumnName("productBrandName");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(50)
                .HasColumnName("website_url");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PK__ProductC__A944253B2847BF0F");

            entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");
            entity.Property(e => e.CategoryCode)
                .HasMaxLength(50)
                .HasColumnName("category_code");
            entity.Property(e => e.CategoryLevel).HasColumnName("category_level");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.MetaKeywords)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("meta_keywords");
            entity.Property(e => e.ProductCategoryDescription).HasColumnName("productCategoryDescription");
            entity.Property(e => e.ProductCategoryName)
                .HasMaxLength(255)
                .HasColumnName("productCategoryName");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.ThumbnailImage)
                .HasMaxLength(50)
                .HasColumnName("thumbnailImage");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
