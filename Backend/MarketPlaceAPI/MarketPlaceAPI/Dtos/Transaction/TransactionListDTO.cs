namespace MarketPlaceAPI.Dtos
{
    public class TransactionListDTO
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public int ProductId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
