using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MarketPlaceAPI.Data.Interfaces;
using System.Security.Claims;

namespace MarketPlaceAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceRepository _balanceRepository;

        public BalanceController(IBalanceRepository balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var balance = await _balanceRepository.GetBalanceAsync(userId);
            return balance;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] decimal amount)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _balanceRepository.DepositAsync(userId, amount);
            return Ok();
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] decimal amount)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _balanceRepository.WithdrawAsync(userId, amount);
            if (!success)
            {
                return BadRequest("Insufficient balance.");
            }
            return Ok();
        }
    }
}
