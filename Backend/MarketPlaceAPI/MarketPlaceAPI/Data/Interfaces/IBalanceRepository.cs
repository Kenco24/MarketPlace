using System.Threading.Tasks;
using MarketPlaceAPI.Data.Models;

namespace MarketPlaceAPI.Data.Interfaces
{
    public interface IBalanceRepository
    {
        Task<decimal> GetBalanceAsync(string userId);
        Task DepositAsync(string userId, decimal amount);
        Task<bool> WithdrawAsync(string userId, decimal amount);
    }
}
