using BuildingBlocks.CQRS;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Test.API.Services;

namespace Test.API.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenResult>;
public record RefreshTokenResult(string AccessToken);

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("RefreshToken is required");
    }
}

internal class RefreshTokenHandler(TokenService tokenService) 
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("your-refresh-token-secret-is-a-32-byte-long-key-12345678");

        var principal = tokenHandler.ValidateToken(command.RefreshToken, new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "me-core",
            ValidAudience = "me-core",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        }, out SecurityToken validatedToken);

        if (validatedToken == null || !(validatedToken is JwtSecurityToken))
        {
            throw new SecurityTokenException("Invalid token");
        }

        // Generate new access token
        var newAccessToken = tokenService.GenerateAccessToken();

        return new RefreshTokenResult(newAccessToken);
    }
}
