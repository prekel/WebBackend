using Microsoft.EntityFrameworkCore;

using MyStore.Data.Entity;

namespace MyStore.Data.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Cart>()
                .HasData(new Cart { CartId = 1 });

            modelBuilder
                .Entity<Product>()
                .HasData(new Product { ProductId = 1 });

            modelBuilder
                .Entity<Cart>()
                .HasMany(cart => cart.Products)
                .WithMany(p => p.Posts)
                .UsingEntity(j => j.HasData(new { PostsPostId = 1, TagsTagId = "ef" }));
        }
    }
}
