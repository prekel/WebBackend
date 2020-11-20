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
            populater.PopulateCustomers(200);
            populater.PopulateProducts(200);
            populater.PopulateCarts(200, 2, 3);
            populater.PopulateOrdersOrderedProducts(300, 4);
            populater.PopulateSupportOperators(50);
            populater.PopulateSupportTickets(70);
            populater.PopulateAnswersQuestions();
        }
    }
}
