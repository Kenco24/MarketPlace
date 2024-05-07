using MarketPlaceAPI.Data.Models;

namespace MarketPlaceAPI.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<IEnumerable<Product>> GetByUserIdAsync(string userId); 
        Task<Product> GetByIdAsync(int productId); 
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int productId);

        Task UpdateProductAsSoldAsync(int productId);
    }

}
