using Microsoft.AspNetCore.Identity;

using MyStore.Data.Shop;
using MyStore.Data.Support;

namespace MyStore.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public int? OperatorId { get; set; }

        public Operator? Operator { get; set; }
    }
}
