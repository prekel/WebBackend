using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using MyStore.Data;

namespace MyStore.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new Context(new DbContextOptions<Context>());

            context.Database.EnsureCreated();

            context.SaveChanges();

            var pr = context.Carts.Select(cart => cart.Products).ToList();
            
            System.Console.WriteLine("Hello World!");
        }
    }
}
