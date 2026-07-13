using Fiap.GestaoFinanca.Api.Middlewares;

namespace Fiap.GestaoFinanca.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseApiMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();

            return app;
        }

    }
}
