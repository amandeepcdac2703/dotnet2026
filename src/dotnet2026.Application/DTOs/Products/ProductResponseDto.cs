using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet2026.Application.DTOs.Products
{
    public sealed record ProductResponseDto(
       long Id,
       string Name,
       string? Description,
       decimal Price,
       int Stock,
       DateTime CreatedAtUtc,
       DateTime? UpdatedAtUtc);
}
