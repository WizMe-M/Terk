namespace Terk.DB.Context;

/// <summary>
/// Context to interact with Terk's database
/// </summary>
public partial class TerkDbContext : DbContext
{
    public TerkDbContext()
    {
    }

    public TerkDbContext(DbContextOptions<TerkDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; } = null!;

    public virtual DbSet<OrderPosition> OrderPositions { get; set; } = null!;

    public virtual DbSet<Product> Products { get; set; } = null!;

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_order");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_customer");
        });

        modelBuilder.Entity<OrderPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_order_position");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPositions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_position_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderPositions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_position_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_product");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
