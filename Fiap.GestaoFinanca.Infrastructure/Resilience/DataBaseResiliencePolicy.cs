using Microsoft.Extensions.Logging;
using Polly;

namespace Fiap.GestaoFinanca.Infrastructure.Resilience
{
    public static class DataBaseResiliencePolicy
    {
        public static IAsyncPolicy CreateRetryPolicy(ILogger logger)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attemp => TimeSpan.FromSeconds(attemp),
                onRetry: (exception,delay,retryAttemp,context) =>
                {
                    logger.LogWarning(
                        exception,
                        "Falha transitoria ao acessar o bacno de dados. Tentativa {RetryAttemp} após {Delay} segundos.",
                        retryAttemp,
                        delay.TotalSeconds
                        );
                });
        }

        public static IAsyncPolicy CreateCircuitBreakPolicy(ILogger logger)
        {

            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (exception, duration) =>
                    {
                        logger.LogError(
                            exception,
                            "Circuit break aberto por  {Duration} segundos devido a falhas consecutivas.",
                            duration.TotalSeconds);
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuito fechado. Operações normalizadas");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogInformation("Circuito em estado de half open. Realizando nova tentativa");
                    }
                );
        }

        public static IAsyncPolicy CreateCombinePolicy(ILogger logger)
        {
            var retryPolicy = CreateRetryPolicy(logger);
            var circuitBreakerPolicy = CreateCircuitBreakPolicy(logger);

            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

        }





    }
}
