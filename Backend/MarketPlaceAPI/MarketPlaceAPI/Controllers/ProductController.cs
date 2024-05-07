using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MarketPlaceAPI.Data.Interfaces;
using MarketPlaceAPI.Data.Models;
using MarketPlaceAPI.Data.Repositories;
using MarketPlaceAPI.Dtos.Product;
using MarketPlaceAPI.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketPlaceAPI.Controllers
{
    // Add Authorize attribute for authorization
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<AppUser> _userManager;


        public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository, UserManager<AppUser> userManager)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }


        [Authorize]
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(ProductCreateDTO productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming user ID is in a claim

                if (_categoryRepository.GetCategoryById(productDto.CategoryId)==null)
                {
                    return NotFound("Category does not exist");
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryId = productDto.CategoryId,
                    SellerId = userId,

                };

                // Handle image upload
                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    using (var stream = productDto.Image.OpenReadStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            product.ImageData = memoryStream.ToArray();
                        }
                    }
                }

                await _productRepository.AddAsync(product);

                return Ok("product created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding a product. -" + ex);
            }
        }




        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                // Retrieve all products from the repository
                var products = await _productRepository.GetAllProductsAsync();

                // Map the products to ProductListDTO
                var productListDto = new List<ProductListDTO>();

                foreach (var product in products)
                {
                    // Get seller details by sellerId
                    var seller = await _userManager.FindByIdAsync(product.SellerId);

                    // Create ProductListDTO object and populate its properties
                    var productDto = new ProductListDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        CategoryName = _categoryRepository.GetCategoryById(product.CategoryId)?.Name,
                        IsSold = product.IsSold,
                        ImageUrl = $"https://localhost:7182/api/Seller/{product.Id}/image",
                        SellerFullName = seller?.FullName 
                    };

                    productListDto.Add(productDto);
                }

                return Ok(productListDto);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving products. -" + ex);
            }
        }


        [HttpGet("{productId}/image")]
        public async Task<IActionResult> GetProductImage(int productId)
        {
            try
            {
                // Retrieve the product from the repository
                var product = await _productRepository.GetByIdAsync(productId);

                if (product == null || product.ImageData == null)
                {
                    return NotFound(); // Return 404 Not Found if product or image not found
                }

                // Return the image data as a file response
                return File(product.ImageData, "image/jpeg"); // Adjust content type if needed
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error fetching product image: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the product image.");
            }
        }

        [HttpGet("{userId}/user")]
        public async Task<IActionResult> GetProductsByUser(string userId)
        {
            try
            {
                // Retrieve products by user ID from the repository
                var products = await _productRepository.GetByUserIdAsync(userId);

                // Map the products to ProductListDTO
                var productListDto = new List<ProductListDTO>();

                foreach (var product in products)
                {
                    // Get seller details by sellerId
                    var seller = await _userManager.FindByIdAsync(product.SellerId);

                    // Create ProductListDTO object and populate its properties
                    var productDto = new ProductListDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        CategoryName = _categoryRepository.GetCategoryById(product.CategoryId)?.Name,
                        IsSold = product.IsSold,
                        ImageUrl = $"https://localhost:7182/api/Seller/{product.Id}/image",
                        SellerFullName = seller?.FullName // Include seller's full name
                    };

                    productListDto.Add(productDto);
                }

                // Return the mapped products
                return Ok(productListDto);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error fetching products by user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products by user.");
            }
        }



        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                // Retrieve all products from the repository
                var products = await _productRepository.GetAllProductsAsync();

                // Filter products by category ID
                var productsInCategory = products.Where(p => p.CategoryId == categoryId);

                // Map the products to ProductListDTO
                var productListDto = new List<ProductListDTO>();

                foreach (var product in productsInCategory)
                {
                    // Get seller details by sellerId
                    var seller = await _userManager.FindByIdAsync(product.SellerId);

                    // Create ProductListDTO object and populate its properties
                    var productDto = new ProductListDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        CategoryName = _categoryRepository.GetCategoryById(product.CategoryId)?.Name,
                        IsSold = product.IsSold,
                        ImageUrl = $"https://localhost:7182/api/Seller/{product.Id}/image",
                        SellerFullName = seller?.FullName // Include seller's full name
                    };

                    productListDto.Add(productDto);
                }

                // Return the mapped products
                return Ok(productListDto);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error fetching products by category: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching products by category.");
            }
        }




        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                // Retrieve the product from the repository
                var product = await _productRepository.GetByIdAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Return 404 Not Found if product not found
                }

                // Get seller details by sellerId
                var seller = await _userManager.FindByIdAsync(product.SellerId);

                // Convert byte array to a file stream
                var imageStream = new MemoryStream(product.ImageData);

                // Create a DTO to represent the product response (excluding image data)
                var productDto = new ProductListDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    CategoryName = _categoryRepository.GetCategoryById(product.CategoryId)?.Name,
                    ImageUrl = $"https://localhost:7182/api/Seller/{product.Id}/image",
                    SellerFullName = seller?.FullName // Include seller's full name
                };

                // Return the file content result along with the product DTO
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error fetching product: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the product.");
            }
        }



        [Authorize]
        [HttpPut("{productId}")]
        public async Task<IActionResult> EditProduct(int productId, ProductEditDTO productDto)
        {
            try
            {
                // Retrieve the product from the repository
                var product = await _productRepository.GetByIdAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Return 404 Not Found if product not found
                }

                // Update product properties
              
                product.Description = productDto.Description;
                product.Price = productDto.Price;


                if (productDto.Image != null && productDto.Image.Length > 0)
                {
                    using (var stream = productDto.Image.OpenReadStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            product.ImageData = memoryStream.ToArray();
                        }
                    }
                }


                // Update the product in the repository
                await _productRepository.UpdateAsync(product);

                return Ok("Product updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error updating product: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the product.");
            }
        }
        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                // Retrieve the product from the repository
                var product = await _productRepository.GetByIdAsync(productId);

                if (product == null)
                {
                    return NotFound(); // Return 404 Not Found if product not found
                }

                // Delete the product from the repository
                await _productRepository.DeleteAsync(productId);

                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the product.");
            }
        }



    }
}
