using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet2026.Domain.Entities
{
    public sealed class Product
    {
        public long Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public decimal Price { get; private set; }

        public int Stock { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? UpdatedAtUtc { get; private set; }

        // Used when reconstructing an entity from persistence.
        private Product()
        {
        }

        // Creates a new product.
        public Product(
            string name,
            string? description,
            decimal price,
            int stock)
        {
            SetDetails(name, description, price, stock);

            CreatedAtUtc = DateTime.UtcNow;
            UpdatedAtUtc = null;
        }

        // Updates an existing product.
        public void UpdateDetails(
            string name,
            string? description,
            decimal price,
            int stock)
        {
            SetDetails(name, description, price, stock);

            UpdatedAtUtc = DateTime.UtcNow;
        }

        private void SetDetails(
            string name,
            string? description,
            decimal price,
            int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Product name is required.",
                    nameof(name));
            }

            name = name.Trim();

            if (name.Length > 150)
            {
                throw new ArgumentException(
                    "Product name cannot exceed 150 characters.",
                    nameof(name));
            }

            if (description?.Length > 1000)
            {
                throw new ArgumentException(
                    "Description cannot exceed 1000 characters.",
                    nameof(description));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Price must be greater than zero.");
            }

            if (stock < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(stock),
                    "Stock cannot be negative.");
            }

            Name = name;
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
            Price = price;
            Stock = stock;
        }

        // Reconstructs a product from persisted database values.
        public static Product Rehydrate(
            long id,
            string name,
            string? description,
            decimal price,
            int stock,
            DateTime createdAtUtc,
            DateTime? updatedAtUtc)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Product ID must be greater than zero.");
            }

            if (createdAtUtc == default)
            {
                throw new ArgumentException(
                    "Creation timestamp is required.",
                    nameof(createdAtUtc));
            }

            return new Product
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                CreatedAtUtc = createdAtUtc,
                UpdatedAtUtc = updatedAtUtc
            };
        }
    }

}
