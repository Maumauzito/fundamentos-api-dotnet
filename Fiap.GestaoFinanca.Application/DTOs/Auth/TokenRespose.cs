namespace Fiap.GestaoFinanca.Application.DTOs.Auth
{
public record TokenResponse(
    string Token,
    string TokenType,
    DateTime Expiration
    );
}
