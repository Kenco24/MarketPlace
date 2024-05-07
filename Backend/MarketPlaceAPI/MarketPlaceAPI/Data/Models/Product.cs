using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MarketPlaceAPI.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        // Store the image data as a byte array
        public byte[] ImageData { get; set; }

        public int CategoryId { get; set; }  

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }  

        public string SellerId { get; set; }

        [ForeignKey("SellerId")]
        public AppUser Seller { get; set; }

        public bool IsSold { get; set; }

     
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
