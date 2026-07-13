namespace Fiap.GestaoFinanca.Api.Middlewares
{
    public sealed class CorrelationIdMiddleware
    {

        private const string CorrelationIdHeader = "X-Correlation-ID";

        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var corrationId = ObterOuCriarCorralationId(context);

            context.Response.Headers[CorrelationIdHeader] = corrationId;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = corrationId
            }
            ))
            {
                _logger.LogInformation("Inicio da requisição {Method} - {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await _next(context);

                _logger.LogInformation("Finalizando a solicitação {Method} - {Path} com o Status {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode);
            }

        }

        private static string ObterOuCriarCorralationId(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var corralationId )
                && !string.IsNullOrWhiteSpace(corralationId))
            {
                return corralationId.ToString();
            }

            return Guid.NewGuid().ToString();

        }
    }
}
