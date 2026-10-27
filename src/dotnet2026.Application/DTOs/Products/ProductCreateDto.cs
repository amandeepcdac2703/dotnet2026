using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dotnet2026.Application.DTOs.Products
{
    public sealed class ProductCreateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; init; } = string.Empty;

        [StringLength(1000)]
        public string? Description
        {
            get; init;
        }

        [Range(typeof(decimal), "0.01", "9999999999999999.99")]
        public decimal Price
        {
            get; init;
        }

        [Range(0, int.MaxValue)]
        public int Stock
        {
            get; init;
        }
    }
}
