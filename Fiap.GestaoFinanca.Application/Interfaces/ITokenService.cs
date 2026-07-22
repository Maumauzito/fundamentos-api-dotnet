using Fiap.GestaoFinanca.Application.DTOs.Auth;

namespace Fiap.GestaoFinanca.Application.Interfaces
{
    public interface ITokenService
    {
        TokenResponse GerarToken(string usuarioId, string nome, string email, string perfil);
    }
}
