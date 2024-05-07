namespace MarketPlaceAPI.Dtos.Product
{
    public class ProductEditDTO
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
    }
}
