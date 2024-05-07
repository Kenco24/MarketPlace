using Microsoft.AspNetCore.Identity;

namespace MarketPlaceAPI.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        public decimal Balance { get; set; }


        public ICollection<Product> Products { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
