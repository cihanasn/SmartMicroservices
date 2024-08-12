using Carter;
using Mapster;
using MediatR;

namespace Test.API.Features.Auth.RefreshToken;

public record RefreshTokenRequest(string RefreshToken);
public record RefreshTokenResponse(string AccessToken);

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/refresh-token", async (RefreshTokenRequest request, ISender sender) =>
        {
            var command = request.Adapt<RefreshTokenCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<RefreshTokenResponse>();

            return Results.Ok(response);

        }).WithName("RefreshToken")
        .Produces<RefreshTokenResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Refresh Token")
        .WithDescription("Refresh Token");
    }
}
