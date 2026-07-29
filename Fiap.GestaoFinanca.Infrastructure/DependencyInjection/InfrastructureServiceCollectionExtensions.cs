using Fiap.GestaoFinanca.Application.Interfaces;
using Fiap.GestaoFinanca.Application.Respositories;
using Fiap.GestaoFinanca.Infrastructure.Authentication;
using Fiap.GestaoFinanca.Infrastructure.Data;
using Fiap.GestaoFinanca.Infrastructure.Respositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Fiap.GestaoFinanca.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services
            ,IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDespesaRepository, DespesaRepository>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
