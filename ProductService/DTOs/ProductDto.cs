using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
