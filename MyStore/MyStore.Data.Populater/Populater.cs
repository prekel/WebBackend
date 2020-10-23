using System;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;

using MyStore.Data.Entity;
using MyStore.Data.Entity.Support;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace MyStore.Data.Populater
{
    public class Populater
    {
        public Populater(VkApi api) => Api = api;

        private VkApi Api { get; }

        public void PopulateCustomers(int n)
        {
            using var context = new Context();

            var r = new Random();

            var cyrRegexp = new Regex("[А-Яа-яЁе]{3,30}");
            var names = Api.Users.Get(
                    Enumerable.Range(1, n * 5).Select(t => (long) r.Next(1, 620_330_243)),
                    ProfileFields.FirstName | ProfileFields.LastName,
                    NameCase.Nom
                ).Select(user => new {user.FirstName, user.LastName})
                .Where(usernames => cyrRegexp.IsMatch(usernames.FirstName) && cyrRegexp.IsMatch(usernames.LastName))
                .ToList();

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
                    $"{(char) r.Next('А', 'Я')}{String.Join("", Enumerable.Range(0, 8).Select(t => (char) r.Next('а', 'я')))}";
                var product = new Product
                {
                    Name = name,
                    Description = $"Описание товара {name}",
                    Price = r.Next(10, 10000) / (decimal) 10
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

        public void PopulateOrdersOrderedProducts(int n, int m)
        {
            using var context = new Context();

            var r = new Random();

            var customers = context.Customers.ToList();
            var products = context.Products.ToList();

            for (var i = 0; i < n; i++)
            {
                var order = new Order
                {
                    Customer = customers[r.Next(customers.Count - 1)]
                };
                order.OrderedProducts = Enumerable.Range(0, m)
                    .Select(_ => r.Next(products.Count - 1))
                    .Distinct()
                    .Select(ind => products[ind])
                    .Select(product => new OrderedProduct
                    {
                        Product = product,
                        Order = order,
                        OrderedPrice = r.NextDouble() > 0.8 ? product.Price * 0.8m : product.Price
                    })
                    .ToList();

                context.Orders.Add(order);
            }

            context.SaveChanges();
        }

        public void PopulateSupportOperators(int n)
        {
            using var context = new Context();

            var r = new Random();

            var cyrRegexp = new Regex("[А-Яа-яЁе]{3,30}");
            var names = Api.Users.Get(
                    Enumerable.Range(1, n * 5).Select(t => (long) r.Next(1, 620_330_243)),
                    ProfileFields.FirstName | ProfileFields.LastName,
                    NameCase.Nom
                ).Select(user => new {user.FirstName, user.LastName})
                .Where(usernames => cyrRegexp.IsMatch(usernames.FirstName) && cyrRegexp.IsMatch(usernames.LastName))
                .ToList();

            var emailDomains = new[] {"yandex.ru", "gmail.com", "mail.ru", "hotmail.com"};

            for (var i = 0; i < n; i++)
            {
                var salt = Crypto.GenerateSaltForPassword();
                var op = new Operator
                {
                    FirstName = names[r.Next(names.Count - 1)].FirstName,
                    LastName = names[r.Next(names.Count - 1)].LastName,
                    Email =
                        $"{String.Join("", Enumerable.Range(0, 8).Select(t => (char) r.Next('a', 'z')))}{r.Next(100, 999)}@{emailDomains[r.Next(emailDomains.Length - 1)]}",
                    PasswordHash = Crypto.ComputePasswordHash("qwerty", salt),
                    PasswordSalt = salt
                };
                context.SupportOperators.Add(op);
            }

            context.SaveChanges();
        }

        public void PopulateSupportTickets(int n)
        {
            using var context = new Context();

            var r = new Random();

            var customers = context.Customers.ToList();
            var operators = context.SupportOperators.ToList();

            context.SupportTickets.AddRange(
                Enumerable.Range(0, n)
                    .Select(_ => new Ticket
                    {
                        Customer = customers[r.Next(customers.Count - 1)],
                        SupportOperator = operators[r.Next(operators.Count - 1)]
                    })
            );

            context.SaveChanges();
        }

        public void PopulateAnswersQuestions()
        {
            using var context = new Context();

            var r = new Random();

            var tickets = context.SupportTickets
                .Include(ticket => ticket.SupportAnswers)
                .Include(ticket => ticket.SupportQuestions)
                .ToList();
            var ops = context.SupportOperators.ToList();

            foreach (var ticket in tickets)
            {
                var randomstring = String.Join("", Enumerable.Range(0, 8).Select(t => (char) r.Next('а', 'я')));
                ticket.SupportQuestions.Add(new Question
                {
                    SupportTicket = ticket,
                    ReadTimestamp = DateTimeOffset.Now + TimeSpan.FromSeconds(10),
                    Text = $"Вопрос {randomstring}"
                });
                ticket.SupportAnswers.Add(new Answer
                {
                    SupportOperator = r.NextDouble() < 0.9 ? ticket.SupportOperator : ops[r.Next(ops.Count - 1)],
                    SupportTicket = ticket,
                    SendTimestamp = DateTimeOffset.Now + TimeSpan.FromSeconds(15),
                    Text = $"Ответ {randomstring}"
                });
                if (r.NextDouble() > 0.5)
                {
                    var isRead = r.NextDouble() > 0.6;
                    var q = new Question
                    {
                        SupportTicket = ticket,
                        SendTimestamp = DateTimeOffset.Now + TimeSpan.FromSeconds(20),
                        Text = $"Дополнительный вопрос {randomstring}"
                    };
                    if (isRead)
                    {
                        q.ReadTimestamp = DateTimeOffset.Now + TimeSpan.FromSeconds(30);
                    }

                    ticket.SupportQuestions.Add(q);

                    if (r.NextDouble() > 0.5 && isRead)
                    {
                        ticket.SupportAnswers.Add(new Answer
                        {
                            SupportOperator =
                                r.NextDouble() < 0.9 ? ticket.SupportOperator : ops[r.Next(ops.Count - 1)],
                            SupportTicket = ticket,
                            SendTimestamp = DateTimeOffset.Now + TimeSpan.FromSeconds(35),
                            Text = $"Ответ на дополнительный вопрос {randomstring}"
                        });
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
