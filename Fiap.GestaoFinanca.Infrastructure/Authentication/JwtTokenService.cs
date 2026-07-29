using Fiap.GestaoFinanca.Application.DTOs.Auth;
using Fiap.GestaoFinanca.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace Fiap.GestaoFinanca.Infrastructure.Authentication
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;


        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }


        public TokenResponse GerarToken(string usuarioId, string nome, string email, string perfil)
        {

            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

            var clains = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuarioId),
                new(JwtRegisteredClaimNames.Name, nome),
                new(JwtRegisteredClaimNames.Email, email),
                new(ClaimTypes.Role, perfil),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: clains,
                expires: expiration,
                signingCredentials: credenciais
                );

            var accsseToken = new JwtSecurityTokenHandler().WriteToken(token);



            return new TokenResponse
            {
                Token = accsseToken,
                TokenType = "Bearer",
                Expiration = expiration
            };


        }


    }
}
