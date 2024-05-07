using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceAPI.Data.Models;

namespace MarketPlaceAPI.Data.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
    }
}
