using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Services;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(new { success = true, data = products, count = products.Count() });
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving products: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { success = false, message = "Product ID must be greater than 0" });
                }

                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { success = false, message = $"Product with ID {id} not found" });
                }

                return Ok(new { success = true, data = product });
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving product: {ex.Message}");
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    return BadRequest(new { success = false, message = "Category cannot be empty" });
                }

                var products = await _productService.GetProductsByCategoryAsync(category);
                return Ok(new { success = true, data = products, count = products.Count(), category });
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving products by category: {ex.Message}");
            }
        }

        [HttpGet("brand/{brand}")]
        public async Task<IActionResult> GetProductsByBrand(string brand)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(brand))
                {
                    return BadRequest(new { success = false, message = "Brand cannot be empty" });
                }

                var products = await _productService.GetProductsByBrandAsync(brand);
                return Ok(new { success = true, data = products, count = products.Count(), brand });
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving products by brand: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                // Validate the DTO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(createProductDto);

                if (!Validator.TryValidateObject(createProductDto, validationContext, validationResults, true))
                {
                    var errors = validationResults.Select(v => v.ErrorMessage).ToList();
                    return BadRequest(new { success = false, message = "Validation failed", errors });
                }

                var product = await _productService.CreateProductAsync(createProductDto);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id },
                    new { success = true, data = product, message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                return Problem($"Error creating product: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { success = false, message = "Product ID must be greater than 0" });
                }

                // Validate the DTO
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(updateProductDto);

                if (!Validator.TryValidateObject(updateProductDto, validationContext, validationResults, true))
                {
                    var errors = validationResults.Select(v => v.ErrorMessage).ToList();
                    return BadRequest(new { success = false, message = "Validation failed", errors });
                }

                var product = await _productService.UpdateProductAsync(id, updateProductDto);
                if (product == null)
                {
                    return NotFound(new { success = false, message = $"Product with ID {id} not found" });
                }

                return Ok(new { success = true, data = product, message = "Product updated successfully" });
            }
            catch (Exception ex)
            {
                return Problem($"Error updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { success = false, message = "Product ID must be greater than 0" });
                }

                var deleted = await _productService.DeleteProductAsync(id);
                if (!deleted)
                {
                    return NotFound(new { success = false, message = $"Product with ID {id} not found" });
                }

                return Ok(new { success = true, message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return Problem($"Error deleting product: {ex.Message}");
            }
        }

        [HttpGet("search/{term}")]
        public async Task<IActionResult> SearchProducts(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return BadRequest(new { success = false, message = "Search term cannot be empty" });
                }

                var allProducts = await _productService.GetAllProductsAsync();
                var filteredProducts = allProducts.Where(p =>
                    p.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(term, StringComparison.OrdinalIgnoreCase));

                return Ok(new { success = true, data = filteredProducts, count = filteredProducts.Count(), searchTerm = term });
            }
            catch (Exception ex)
            {
                return Problem($"Error searching products: {ex.Message}");
            }
        }

        [HttpGet("price-range")]
        public async Task<IActionResult> GetProductsByPriceRange([FromQuery] decimal? min, [FromQuery] decimal? max)
        {
            try
            {
                var allProducts = await _productService.GetAllProductsAsync();
                var filteredProducts = allProducts.AsQueryable();

                if (min.HasValue)
                {
                    if (min.Value < 0)
                    {
                        return BadRequest(new { success = false, message = "Minimum price cannot be negative" });
                    }
                    filteredProducts = filteredProducts.Where(p => p.Price >= min.Value);
                }

                if (max.HasValue)
                {
                    if (max.Value < 0)
                    {
                        return BadRequest(new { success = false, message = "Maximum price cannot be negative" });
                    }
                    filteredProducts = filteredProducts.Where(p => p.Price <= max.Value);
                }

                if (min.HasValue && max.HasValue && min.Value > max.Value)
                {
                    return BadRequest(new { success = false, message = "Minimum price cannot be greater than maximum price" });
                }

                var result = filteredProducts.ToList();
                return Ok(new { success = true, data = result, count = result.Count, priceRange = new { min, max } });
            }
            catch (Exception ex)
            {
                return Problem($"Error filtering products by price: {ex.Message}");
            }
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold)
        {
            try
            {
                if (threshold < 0)
                {
                    return BadRequest(new { success = false, message = "Threshold cannot be negative" });
                }

                var allProducts = await _productService.GetAllProductsAsync();
                var lowStockProducts = allProducts.Where(p => p.StockQuantity <= threshold).ToList();

                return Ok(new { success = true, data = lowStockProducts, count = lowStockProducts.Count, threshold });
            }
            catch (Exception ex)
            {
                return Problem($"Error retrieving low stock products: {ex.Message}");
            }
        }
    }
}
