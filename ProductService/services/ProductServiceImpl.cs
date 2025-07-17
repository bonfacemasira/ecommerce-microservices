using ProductService.DTOs;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceImpl(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            return products.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(string brand)
        {
            var products = await _productRepository.GetByBrandAsync(brand);
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = MapToEntity(createProductDto);
            var createdProduct = await _productRepository.CreateAsync(product);
            return MapToDto(createdProduct);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return null;
            }

            // Update properties
            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.StockQuantity = updateProductDto.StockQuantity;
            existingProduct.Category = updateProductDto.Category;
            existingProduct.Brand = updateProductDto.Brand;
            existingProduct.ImageUrl = updateProductDto.ImageUrl;
            existingProduct.IsActive = updateProductDto.IsActive;

            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
            return MapToDto(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Category = product.Category,
                Brand = product.Brand,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        private static Product MapToEntity(CreateProductDto createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                Category = createProductDto.Category,
                Brand = createProductDto.Brand,
                ImageUrl = createProductDto.ImageUrl,
                IsActive = createProductDto.IsActive
            };
        }
    }
}
