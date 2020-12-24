using VkNet;
using VkNet.Enums;
using VkNet.Model;

namespace MyStore.Data.Populater
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (var context = new Context())
            {
                context.Database.EnsureCreated();
            }

            var api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                AccessToken = "1bb9ca221bb9ca221bb9ca22ad1bdfa76e11bb91bb9ca22441bbfc7d2cfe35c00c4a071"
            });
            api.SetLanguage(Language.Ru);

            var populater = new Populater(api);
            var c = 4000;
            for (var i = 72000; i < 500000; i += c)
            {
                populater.PopulateCustomers(c);
            }

            populater.PopulateProducts(500000);
            populater.PopulateCarts(500000, 2, 3);
            populater.PopulateOrdersOrderedProducts(600000, 4);

            for (var i = 0; i < 125000; i += c)
            {
                populater.PopulateSupportOperators(c);
            }

            populater.PopulateSupportTickets(150000);
            populater.PopulateAnswersQuestions();
        }
    }
}
