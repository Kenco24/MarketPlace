using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MarketPlaceAPI.Data.Interfaces;
using MarketPlaceAPI.Data.Models;
using MarketPlaceAPI.Data.Repositories;
using MarketPlaceAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBalanceRepository _balanceRepository;

        public TransactionController(ITransactionRepository transactionRepository, IProductRepository productRepository, IBalanceRepository balanceRepository)
        {
            _transactionRepository = transactionRepository;
            _productRepository = productRepository;
            _balanceRepository = balanceRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction(int productId)
        {
            try
            {
                // Fetch buyer ID from claims
                var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Check if the buyer is not the seller
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                if (product.SellerId == buyerId)
                {
                    return BadRequest("You cannot buy your own product.");
                }

                // Get buyer's balance
                var buyerBalance = await _balanceRepository.GetBalanceAsync(buyerId);
                if (buyerBalance == null)
                {
                    return NotFound("Buyer's balance not found.");
                }

                // Check if buyer has enough balance
                if (buyerBalance <= 0)
                {
                    return BadRequest("Insufficient balance to make the purchase.");
                }

                // Check if the buyer can afford the product
                if (buyerBalance < product.Price)
                {
                    return BadRequest("Insufficient balance to buy this product.");
                }

                // Proceed with creating the transaction
                var transaction = new Transaction
                {
                    BuyerId = buyerId,
                    ProductId = productId,
                    TransactionDate = DateTime.Now
                };

                await _transactionRepository.AddTransactionAsync(transaction);

                // Mark the product as sold
                await _productRepository.UpdateProductAsSoldAsync(productId);

                return Ok("Transaction created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the transaction: {ex.Message}");
            }
        }

        [HttpGet("userTransactions")]
        public async Task<IActionResult> GetUserTransactions()
        {
            try
            {
                // Get the user ID from claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID not found in claims.");
                }

                // Fetch transactions by user ID
                var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching user transactions: {ex.Message}");
            }
        }




        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
                if (transaction == null)
                {
                    return NotFound("Transaction not found");
                }

                var transactionDto = new TransactionListDTO
                {
                    Id = transaction.Id,
                    BuyerId = transaction.BuyerId,
                    ProductId = transaction.ProductId,
                    TransactionDate = transaction.TransactionDate,
                    Price = transaction.Price,
                    Amount = transaction.Amount
                };

                return Ok(transactionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the transaction: {ex.Message}");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionRepository.GetAllTransactionsAsync();
                var transactionDtos = new List<TransactionListDTO>();

                foreach (var transaction in transactions)
                {
                    var transactionDto = new TransactionListDTO
                    {
                        Id = transaction.Id,
                        BuyerId = transaction.BuyerId,
                        ProductId = transaction.ProductId,
                        TransactionDate = transaction.TransactionDate,
                        Price = transaction.Price,
                        Amount = transaction.Amount
                    };

                    transactionDtos.Add(transactionDto);
                }

                return Ok(transactionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching transactions: {ex.Message}");
            }
        }
    }
}
