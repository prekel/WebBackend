using System;

using Microsoft.EntityFrameworkCore;

using MyStore.Data.Identity;
using MyStore.Data.Shop;
using MyStore.Data.Support;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyStore.Data
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartProduct> CartProducts { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderedProduct> OrderedProducts { get; set; } = null!;
        public DbSet<Answer> SupportAnswers { get; set; } = null!;
        public DbSet<Operator> SupportOperators { get; set; } = null!;
        public DbSet<Question> SupportQuestions { get; set; } = null!;
        public DbSet<Ticket> SupportTickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(eb =>
            {
                eb.HasOne(user => user.Customer)
                    .WithOne(customer => customer.User)
                    .HasForeignKey<ApplicationUser>(user => user.CustomerId)
                    .IsRequired(false);
                eb.HasOne(user => user.Operator)
                    .WithOne(operator1 => operator1.User)
                    .HasForeignKey<ApplicationUser>(user => user.OperatorId)
                    .IsRequired(false);
            });

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
                    e.HasOne(entity => entity.CurrentCart)
                        .WithMany(cart => cart!.CurrentCustomers)
                        .HasForeignKey(customer => customer.CurrentCartId)
                        .IsRequired(false);
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
                        .HasColumnType("numeric(20, 2)")
                        .IsRequired();
                });

            modelBuilder.Entity<Cart>(
                e =>
                {
                    e.HasKey(cart => cart.CartId);
                    e.HasMany(cart => cart.Products)
                        .WithMany(product => product.Carts)
                        .UsingEntity<CartProduct>(
                            j => j
                                .HasOne(cp => cp.Product)
                                .WithMany(p => p.CartProducts)
                                .HasForeignKey(cp => cp.ProductId),
                            j => j
                                .HasOne(cp => cp.Cart)
                                .WithMany(c => c.CartProducts)
                                .HasForeignKey(cp => cp.CartId),
                            j => { j.HasKey(cp => new {cp.CartId, cp.ProductId}); });
                    e.HasOne(cart => cart.OwnerCustomer)
                        .WithMany(customer => customer!.OwnedCarts)
                        .HasForeignKey(cart => cart.OwnerCustomerId);
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
                    e.HasKey(op => new {op.ProductId, op.OrderId});
                    e.HasOne(op => op.Product)
                        .WithMany(p => p.OrderedProducts)
                        .HasForeignKey(op => op.ProductId);
                    e.Property(op => op.OrderedPrice)
                        .HasColumnType("numeric(20, 2)")
                        .IsRequired();
                    e.HasOne(op => op.Order)
                        .WithMany(o => o.OrderedProducts)
                        .HasForeignKey(op => op.OrderId);
                });

            modelBuilder.Entity<Answer>(
                b =>
                {
                    b.HasKey(answer => answer.SupportAnswerId);
                    b.HasOne(answer => answer.SupportOperator)
                        .WithMany(op => op.SupportAnswers)
                        .HasForeignKey(answer => answer.SupportOperatorId);
                    b.HasOne(answer => answer.SupportTicket)
                        .WithMany(ticket => ticket.SupportAnswers)
                        .HasForeignKey(answer => answer.SupportTicketId);
                    b.Property(answer => answer.SendTimestamp)
                        .HasDefaultValueSql("current_timestamp")
                        .IsRequired();
                    b.Property(answer => answer.Text)
                        .IsRequired();
                });

            modelBuilder.Entity<Ticket>(
                b =>
                {
                    b.HasKey(ticket => ticket.SupportTicketId);
                    b.HasOne(ticket => ticket.SupportOperator)
                        .WithMany(op => op.SupportTickets)
                        .HasForeignKey(ticket => ticket.SupportOperatorId);
                    b.HasOne(ticket => ticket.Customer)
                        .WithMany(customer => customer.SupportTickets)
                        .HasForeignKey(ticket => ticket.CustomerId);
                    b.Property(ticket => ticket.CreateTimestamp)
                        .HasDefaultValueSql("current_timestamp")
                        .IsRequired();
                    b.HasOne(ticket => ticket.Order)
                        .WithOne(order => order!.SupportTicket!)
                        .HasForeignKey<Ticket>(ticket => ticket.OrderId)
                        .IsRequired(false);
                });

            modelBuilder.Entity<Operator>(
                b =>
                {
                    b.HasKey(op => op.SupportOperatorId);
                    b.Property(op => op.FirstName)
                        .HasMaxLength(60)
                        .IsRequired();
                    b.Property(op => op.LastName)
                        .HasMaxLength(60)
                        .IsRequired();
                    b.Property(op => op.Email)
                        .HasMaxLength(60)
                        .IsRequired();
                });

            modelBuilder.Entity<Question>(b =>
            {
                b.HasKey(question => question.SupportQuestionId);
                b.HasOne(question => question.SupportTicket)
                    .WithMany(ticket => ticket.SupportQuestions)
                    .HasForeignKey(question => question.SupportTicketId);
                b.Property(question => question.SendTimestamp)
                    .HasDefaultValueSql("current_timestamp")
                    .IsRequired();
                b.Property(question => question.ReadTimestamp);
                b.Property(question => question.Text)
                    .IsRequired();
            });
        }
    }
}
