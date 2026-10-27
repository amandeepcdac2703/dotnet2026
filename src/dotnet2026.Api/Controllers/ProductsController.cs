using Asp.Versioning;
using dotnet2026.Application.DTOs.Products;
using dotnet2026.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet2026.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductResponseDto>), 200)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var products = await _service.GetAllAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(
            long id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Id must be greater than zero."
                });

            var product = await _service.GetByIdAsync(id, cancellationToken);

            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductResponseDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(
            [FromBody] ProductCreateDto dto,
            CancellationToken cancellationToken)
        {
            var product = await _service.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = product.Id
                },
                product);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(
            long id,
            [FromBody] ProductUpdateDto dto,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Id must be greater than zero."
                });

            bool updated = await _service.UpdateAsync(
                id, dto, cancellationToken);

            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(
            long id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Id must be greater than zero."
                });

            bool deleted = await _service.DeleteAsync(id, cancellationToken);

            return deleted ? NoContent() : NotFound();
        }
    }
}
