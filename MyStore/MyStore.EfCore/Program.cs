using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyStore.Data;
using MyStore.Data.Entity;

Console.OutputEncoding = Encoding.UTF8;

using var context = new Context();

string GetCustomerFirstName(int id)
{
    return context.Customers.First(customer => customer.CustomerId == id).FirstName;
}

Console.WriteLine("Имя покупателя с id=2");
Console.WriteLine(GetCustomerFirstName(2));
Console.WriteLine();

IEnumerable<string> GetCustomerFirstNames(int startId, int endId)
{
    return context.Customers
        .Where(customer => startId <= customer.CustomerId && customer.CustomerId <= endId)
        .Select(customer => customer.FirstName);
}

Console.WriteLine("Имена покупателей с id от 4 по 9");
foreach (var i in GetCustomerFirstNames(4, 9))
{
    Console.WriteLine(i);
}

Console.WriteLine();

int AddCustomer(Customer customer)
{
    var c1 = context.Customers.Add(customer);
    context.SaveChanges();
    return c1.Entity.CustomerId;
}

var salt = Crypto.GenerateSaltForPassword();
var newId = AddCustomer(new Customer
{
    FirstName = "Тимофей",
    LastName = "Тимофеев",
    Honorific = "Даыо.",
    Email = "dawfdfwa@fsaf.ru",
    PasswordHash = Crypto.ComputePasswordHash("123456", salt),
    PasswordSalt = salt
});

Console.WriteLine($"Создан новый покупатель с id={newId} и именем {GetCustomerFirstName(newId)}");
