namespace Fiap.GestaoFinanca.Application.DTOs.Auth
{
    public sealed record TokenResponse
    {
        public string Token { get; init; } = string.Empty;
        public string TokenType { get; init; } = string.Empty;
        public DateTime Expiration { get; init; }
    }
}
