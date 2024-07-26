using BuildingBlocks.CQRS;
using Test.API.Models;

namespace Test.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);
internal class GetProductsQueryHandler()
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Price = 10.99m },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Price = 15.99m },
                new Product { Id = Guid.NewGuid(), Name = "Product 3", Price = 20.99m }
            };

        var pagedProducts = products
            .Skip((query.PageNumber.GetValueOrDefault(1) - 1) * query.PageSize.GetValueOrDefault(10))
            .Take(query.PageSize.GetValueOrDefault(10))
            .ToList();

        return new GetProductsResult(pagedProducts);
    }
}

