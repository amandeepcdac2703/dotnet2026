using dotnet2026.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet2026.Application.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(
            CancellationToken cancellationToken);

        Task<ProductResponseDto?> GetByIdAsync(
            long id, CancellationToken cancellationToken);

        Task<ProductResponseDto> CreateAsync(
            ProductCreateDto dto, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(
            long id, ProductUpdateDto dto, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
            long id, CancellationToken cancellationToken);
    }
}
