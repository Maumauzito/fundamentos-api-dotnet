using Fiap.GestaoFinanca.Application.DTOs.Auth;
using Fiap.GestaoFinanca.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fiap.GestaoFinanca.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth")
                .WithTags("Autenticação");
              

            group.MapPost("/login", (LoginRequest request, ITokenService tokenService) =>
            {
                const string emailValido = "admin@fiap.com.br";
                const string senhaValida = "FIAP@2026";

                if (request.Email != emailValido || request.Senha != senhaValida)
                {
                    return Results.Unauthorized();
                }

                var token = tokenService.GerarToken(
                    usuarioId: "1",
                    nome: "Administrador FIAP",
                    email: request.Email,
                    perfil: "Admin");

                return Results.Ok(token);
            })
            .WithName("Login")
            .WithSummary("Autentica um usuário e retorna um token JWT")
            .WithDescription("Recebe e-mail e senha, valida as credenciais e retorna um token JWT para acesso aos endpoints protegidos.")
            .Accepts<LoginRequest>("application/json")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

            group.MapGet("/me", (ClaimsPrincipal user) =>
            {
                var nome = user.FindFirstValue(JwtRegisteredClaimNames.Name);
                var email = user.FindFirstValue(JwtRegisteredClaimNames.Email);
                var perfil = user.FindFirstValue(ClaimTypes.Role);

                return Results.Ok(new
                {
                    Nome = nome,
                    Email = email,
                    Perfil = perfil
                });
            })
            .RequireAuthorization()
            .WithName("UsuarioAutenticado")
            .WithSummary("Retorna informações do usuário autenticado")
            .WithDescription("Lê as claims presentes no token JWT e retorna os dados principais do usuário autenticado.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);


            return group;
        }



    }
}
