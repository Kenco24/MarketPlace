namespace MarketPlaceAPI.Dtos.Product
{
    public class ProductListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsSold { get; set; }
        public string ImageUrl { get; set; }

        public string SellerFullName {  get; set; }


    }
}
