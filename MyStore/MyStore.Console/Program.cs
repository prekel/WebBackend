using System;
using System.Linq;
using System.Text;

using Microsoft.EntityFrameworkCore;

using MyStore.Data;
using MyStore.Data.Entity;

namespace MyStore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.UTF8;

            using var context = new Context(new DbContextOptions<Context>());

            context.Database.EnsureCreated();
            context.SaveChanges();

            // var c1 = context.Carts.ToList();
            // var c2 = context.Customers.ToList();
            //
            // context.Customers.AddRange(
            //     new Customer()
            //     {
            //         FirstName = "Name1",
            //         Email = "qwe",
            //         PasswordHash = new byte[] {0, 1},
            //         PasswordSalt = 123
            //     },
            //     new Customer()
            //     {
            //         FirstName = "Name2",
            //         Email = "qwe",
            //         PasswordHash = new byte[] {0, 2},
            //         PasswordSalt = 124
            //     });
            // context.Carts.AddRange(
            //     new Cart(), new Cart());
            // context.SaveChanges();
            //
            //
            // context.Carts.First().OwnerCustomer = context.Customers.First();
            // context.SaveChanges();
            //
            // context.Customers.First().CurrentCart = context.Carts.First();
            // context.SaveChanges();
            //
            // System.Console.WriteLine("Hello World!");
        }
    }
}
