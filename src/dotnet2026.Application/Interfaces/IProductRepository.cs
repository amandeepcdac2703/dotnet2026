using dotnet2026.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet2026.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllAsync(
            CancellationToken cancellationToken);

        Task<Product?> GetByIdAsync(
            long id,
            CancellationToken cancellationToken);

        Task<long> CreateAsync(
            Product product,
            CancellationToken cancellationToken);

        Task<bool> UpdateAsync(
            Product product,
            CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
            long id,
            CancellationToken cancellationToken);
    }
}
