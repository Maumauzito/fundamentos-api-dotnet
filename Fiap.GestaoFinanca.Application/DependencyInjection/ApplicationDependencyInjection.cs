using Fiap.GestaoFinanca.Application.Interfaces;
using Fiap.GestaoFinanca.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.GestaoFinanca.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDespesaService, DespesaService>();

            return services;
        }

    }
}
