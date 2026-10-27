using dotnet2026.Application.DTOs.Products;
using dotnet2026.Application.Interfaces;
using dotnet2026.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet2026.Application.Features
{
    public sealed class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        private static ProductResponseDto Map(Product p) =>
            new(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.CreatedAtUtc,
                p.UpdatedAtUtc);

        public async Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync(cancellationToken);

            return products.Select(Map).ToList();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(
            long id, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);

            return product is null ? null : Map(product);
        }

        public async Task<ProductResponseDto> CreateAsync(
            ProductCreateDto dto, CancellationToken cancellationToken)
        {
            var product = new Product(
                dto.Name, dto.Description, dto.Price, dto.Stock);

            long id = await _repository.CreateAsync(product, cancellationToken);

            var created = await _repository.GetByIdAsync(id, cancellationToken);

            if (created is null)
                throw new InvalidOperationException(
                    "The product was created but could not be retrieved.");

            return Map(created);
        }

        public async Task<bool> UpdateAsync(
            long id, ProductUpdateDto dto, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);

            if (product is null)
                return false;

            product.UpdateDetails(
                dto.Name, dto.Description, dto.Price, dto.Stock);

            return await _repository.UpdateAsync(product, cancellationToken);
        }

        public Task<bool> DeleteAsync(
            long id, CancellationToken cancellationToken) =>
            _repository.DeleteAsync(id, cancellationToken);
    }
}
