using Microsoft.EntityFrameworkCore;
using StockManagement.Data.Entities;

namespace StockManagement.Data;

public partial class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Purchase> Purchases { get; set; }
    public virtual DbSet<Sale> Sales { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Category");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(150);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Customer");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Customertitle).HasColumnName("CUSTOMERTITLE").HasMaxLength(50);
            entity.Property(e => e.Customernumber).HasColumnName("CUSTOMERNUMBER").HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Product");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(150);
            entity.Property(e => e.ImageSrc).HasColumnName("IMAGE_SRC").HasMaxLength(100);
            entity.Property(e => e.Salesprice).HasColumnName("SALESPRICE");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Purchase");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
            entity.Property(e => e.Price).HasColumnName("PRICE");
            entity.Property(e => e.Amount).HasColumnName("AMOUNT");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.CustomerId).HasColumnName("CUSTOMER_ID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Purchase_Customer");

            entity.HasOne(d => d.Product).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Purchase_Product");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Sales");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
            entity.Property(e => e.Salesprice).HasColumnName("SALESPRICE");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.Amount).HasColumnName("AMOUNT");
            entity.Property(e => e.CustomerId).HasColumnName("CUSTOMER_ID");
            entity.Property(e => e.Listprice).HasColumnName("LISTPRICE");
            entity.Property(e => e.Discountrate).HasColumnName("DISCOUNTRATE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Sales_Customer");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Sales_Product");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Stock");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("PRODUCT_ID");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
            entity.Property(e => e.Date).HasColumnName("DATE");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Stock_Product");
        });

        modelBuilder.Entity<PurchaseListDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<SalesListDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<SalesReportDto>().HasNoKey().ToView(null);
        modelBuilder.Entity<StockReportDto>().HasNoKey().ToView(null);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
