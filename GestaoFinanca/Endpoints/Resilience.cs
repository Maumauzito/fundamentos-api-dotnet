using Fiap.GestaoFinanca.Infrastructure.Resilience;

namespace Fiap.GestaoFinanca.Api.Endpoints
{
    public static class Resilience
    {

        public static RouteGroupBuilder MapResilienceEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/resilience")
                        .WithTags("Resilience");
                        

            group.MapGet("/circuit-breaker-demo", async (ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("CircuitBreakerDemo");
                var policy = DataBaseResiliencePolicy.CreateCircuitBreakPolicy(logger);

                await policy.ExecuteAsync(() =>
                {
                    throw new InvalidOperationException("Falha contínua simulada para demonstrar circuit breaker.");
                });

                return Results.Ok("Operação executada com sucesso.");
            })
            .WithName("CircuitBreakerDemo")
            .WithSummary("Demonstra o comportamento de circuit breaker com Polly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
