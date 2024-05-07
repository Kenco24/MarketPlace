using System;
using System.Threading.Tasks;
using MarketPlaceAPI.Data.Interfaces;
using MarketPlaceAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceAPI.Data.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public BalanceRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            return user.Balance;
        }

        public async Task DepositAsync(string userId, decimal amount)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            user.Balance += amount;
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> WithdrawAsync(string userId, decimal amount)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            if (user.Balance < amount)
            {
                return false; // Insufficient balance
            }

            user.Balance -= amount;
            await _userManager.UpdateAsync(user);
            return true;
        }

       

    }
}
