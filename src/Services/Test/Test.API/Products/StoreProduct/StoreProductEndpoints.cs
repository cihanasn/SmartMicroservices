using Carter;
using Mapster;
using MediatR;
using Test.API.Models;
using static Test.API.Products.StoreProduct.StoreProductHandler;

namespace Test.API.Products.StoreProduct;
public record StoreProductRequest(Product Product);
public record StoreProductResponse(Guid Id);
public class StoreProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/product", async (StoreProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<StoreProductCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<StoreProductResponse>();

            return Results.Ok(response);
            
        }).WithName("StoreProduct")
        .Produces<StoreProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}

