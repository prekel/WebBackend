using System;

using Microsoft.EntityFrameworkCore;

using MyStore.Data.Entity;

namespace MyStore.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=qwerty123");
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(
                e =>
                {
                    e.HasKey(entity => entity.CustomerId);
                    e.Property(entity => entity.FirstName)
                        .HasMaxLength(60)
                        .IsRequired();
                    e.Property(entity => entity.LastName)
                        .HasMaxLength(60);
                    e.Property(entity => entity.Honorific)
                        .HasMaxLength(30)
                        .HasDefaultValue("Ув.");
                    e.Property(entity => entity.Email)
                        .HasMaxLength(60)
                        .IsRequired();
                    e.Property(entity => entity.PasswordHash)
                        .HasMaxLength(32)
                        .IsRequired();
                    e.Property(entity => entity.PasswordSalt)
                        .IsRequired();
                    e.HasMany(entity => entity.Carts)
                        .WithMany(cart => cart.Customers)
                        .UsingEntity<CartCustomer>(
                            j => j
                                .HasOne(cc => cc.Cart)
                                .WithMany(c => c.CartCustomers)
                                .HasForeignKey(c => c.CartId), 
                            j => j
                                .HasOne(cc => cc.Customer)
                                .WithMany(c => c.CartCustomers)
                                .HasForeignKey(c => c.CustomerId),
                            j =>
                            {
                                j.HasKey(cc => new {cc.CartId, cc.CustomerId});
                                j.Property(cc => cc.NullIfPublic);
                                j.Property(cc => cc.NullIfNotCurrent);
                                j.HasIndex(ind => new {ind.CartId, ind.NullIfPublic});
                                j.HasIndex(ind => new {ind.CustomerId, ind.NullIfNotCurrent});
                                // TODO
                                j.HasCheckConstraint("Check_NullIfPublic_NotFalse", "NullIfPublic <> false");
                                j.HasCheckConstraint("Check_NullIfNotCurrent_NotFalse", "NullIfNotCurrent <> false");
                            });
                    e.HasOne(customer => customer.CurrentCart)
                        .WithMany(cart => cart.CurrentCustomers)
                        .HasForeignKey(customer => customer.CurrentCartId);
                });

            modelBuilder.Entity<Product>(
                e =>
                {
                    e.HasKey(cart => cart.ProductId);
                    e.Property(cart => cart.Name)
                        .HasMaxLength(100)
                        .IsRequired();
                    e.Property(cart => cart.Description)
                        .IsRequired();
                    e.Property(cart => cart.Price)
                        .HasColumnType("money")
                        .IsRequired();
                });

            modelBuilder.Entity<Cart>(
                e =>
                {
                    e.HasKey(cart => cart.CartId);
                    e.HasMany(cart => cart.Products)
                        .WithMany(product => product.Carts);
                });
            
            modelBuilder.Entity<Order>(
                e =>
                {
                    e.HasKey(order => order.OrderId);
                    e.HasOne(order => order.Customer)
                        .WithMany(customer => customer.Orders)
                        .HasForeignKey(order => order.CustomerId);
                    e.Property(order => order.CreateTimeOffset)
                        .HasDefaultValueSql("current_timestamp")
                        .IsRequired();
                });

            modelBuilder.Entity<OrderedProduct>(
                e =>
                {
                    e.HasKey(op => op.ProductId);
                    e.HasOne(op => op.Product)
                        .WithMany(p => p.OrderedProducts)
                        .HasForeignKey(op => op.ProductId);
                    e.Property(op => op.OrderedPrice)
                        .HasColumnType("money")
                        .IsRequired();
                    e.HasOne(op => op.Order)
                        .WithMany(o => o.OrderedProducts)
                        .HasForeignKey(op => op.OrderId);
                });
        }
    }
}
