
using Dapper;
using dotnet2026.Application.Interfaces;
using dotnet2026.Domain.Entities;
using dotnet2026.Infrastructure.Persistence;

namespace dotnet2026.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private const string SelectColumns = """
        Id, Name, Description, Price, Stock,
        CreatedAtUtc, UpdatedAtUtc
        """;

    private static Product Map(ProductRow row) =>
        Product.Rehydrate(
            row.Id,
            row.Name,
            row.Description,
            row.Price,
            row.Stock,
            row.CreatedAtUtc,
            row.UpdatedAtUtc);

    public async Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, Name, Description, Price, Stock,
                   CreatedAtUtc, UpdatedAtUtc
            FROM Products
            ORDER BY Id DESC;
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            cancellationToken: cancellationToken);

        var rows = await connection.QueryAsync<ProductRow>(command);

        return rows.Select(Map).ToList();
    }

    public async Task<Product?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken)
    {
        var sql = $"""
            SELECT {SelectColumns}
            FROM Products
            WHERE Id = @Id;
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken);

        var row =
            await connection.QuerySingleOrDefaultAsync<ProductRow>(
                command);

        return row is null ? null : Map(row);
    }

    public async Task<long> CreateAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        const string insertSql = """
            INSERT INTO Products
                (Name, Description, Price, Stock,
                 CreatedAtUtc, UpdatedAtUtc)
            VALUES
                (@Name, @Description, @Price, @Stock,
                 UTC_TIMESTAMP(6), NULL);
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var parameters = new
        {
            product.Name,
            product.Description,
            product.Price,
            product.Stock
        };

        var insertCommand = new CommandDefinition(
            insertSql,
            parameters,
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(insertCommand);

        const string idSql = "SELECT LAST_INSERT_ID();";

        var idCommand = new CommandDefinition(
            idSql,
            cancellationToken: cancellationToken);

        return await connection.QuerySingleAsync<long>(idCommand);
    }

    public async Task<bool> UpdateAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE Products
            SET Name = @Name,
                Description = @Description,
                Price = @Price,
                Stock = @Stock,
                UpdatedAtUtc = UTC_TIMESTAMP(6)
            WHERE Id = @Id;
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock
            },
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command) > 0;
    }

    public async Task<bool> DeleteAsync(
        long id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM Products
            WHERE Id = @Id;
            """;

        await using var connection =
            _connectionFactory.CreateConnection();

        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command) > 0;
    }

    private sealed class ProductRow
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }
    }
}
