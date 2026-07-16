using Fiap.GestaoFinanca.Application.Respositories;
using Fiap.GestaoFinanca.Infrastructure.Data;
using Fiap.GestaoFinanca.Infrastructure.Respositories;
using Microsoft.Extensions.DependencyInjection;


namespace Fiap.GestaoFinanca.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDespesaRepository, DespesaRepository>();

            return services;
        }
    }
}
