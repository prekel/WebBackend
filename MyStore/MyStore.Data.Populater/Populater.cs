using System;
using System.Linq;

using VkNet;

using MyStore.Data;
using MyStore.Data.Entity;

using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace MyStore.Data.Populater
{
    public class Populater
    {
        private VkApi Api { get; }

        public Populater(VkApi api)
        {
            Api = api;
        }

        public void PopulateCustomers(int n)
        {
            using var context = new Context();

            var r = new Random();
            var names = Api.Users.Get(Enumerable.Range(1, n * 2).Select(t => (long) r.Next(1, 620_330_243)),
                ProfileFields.FirstName | ProfileFields.LastName, NameCase.Nom);

            var emailDomains = new[] {"yandex.ru", "gmail.com", "mail.ru", "hotmail.com"};

            for (var i = 0; i < n; i++)
            {
                var salt = Crypto.GenerateSaltForPassword();
                var customer = new Customer
                {
                    FirstName = names[r.Next(names.Count - 1)].FirstName,
                    LastName = r.NextDouble() < 0.7 ? names[r.Next(names.Count - 1)].LastName : null,
                    Honorific = r.NextDouble() < 0.1 ? "Дор." : null,
                    Email =
                        $"{String.Join("", Enumerable.Range(0, 8).Select(t => (char) r.Next('a', 'z')))}{r.Next(100, 999)}@{emailDomains[r.Next(emailDomains.Length - 1)]}",
                    PasswordHash = Crypto.ComputePasswordHash("qwerty", salt),
                    PasswordSalt = salt
                };
                context.Customers.Add(customer);
            }

            context.SaveChanges();
        }

        public void PopulateProducts(int n)
        {
            using var context = new Context();

            var r = new Random();

            for (var i = 0; i < n; i++)
            {
                var name =
                    $"{(char) r.Next('а', 'я')}{String.Join("", Enumerable.Range(0, 8).Select(t => (char) r.Next('а', 'я')))}";
                var product = new Product
                {
                    Name = name,
                    Description = $"Описание товара {name}",
                    Price = r.Next(10, 100000000) / (decimal) 10,
                };
                context.Products.Add(product);
            }

            context.SaveChanges();
        }

        public void PopulateCarts(int n, int m, int k)
        {
            using var context = new Context();

            var r = new Random();

            var customers = context.Customers.ToList();
            var products = context.Products.ToList();

            var customersCount = context.Customers.Count();
            var productsCount = context.Products.Count();


            for (var i = 0; i < n; i++)
            {
                var isPublic = r.NextDouble() > 0.7;
                var cart = new Cart
                {
                    IsPublic = isPublic,
                    OwnerCustomer = r.NextDouble() > 0.7 || !isPublic ? customers[r.Next(customersCount - 1)] : null
                };
                for (var j = 0; j < r.Next(m); j++)
                {
                    context.CartProducts.Add(
                        new CartProduct
                        {
                            Cart = cart, 
                            Product = products[r.Next(productsCount - 1)]
                        });
                }

                if (cart.IsPublic)
                {
                    for (var j = 0; j < r.Next(k); j++)
                    {
                        customers[r.Next(customersCount - 1)].CurrentCart = cart;
                    }
                }
                else if (r.NextDouble() > 0.7)
                {
                    cart.OwnerCustomer.CurrentCart = cart;
                }

                context.Carts.Add(cart);
            }

            context.SaveChanges();
        }
    }
}
