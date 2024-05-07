using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlaceAPI.Data.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string BuyerId { get; set; }

        [ForeignKey("BuyerId")]
        public AppUser Buyer { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }


}
