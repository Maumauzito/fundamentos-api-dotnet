using System.Diagnostics;


namespace Fiap.GestaoFinanca.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {


        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Inicio da requisição {Method} - {Path}",
                context.Request.Method,
                context.Request.Path);


            try
            {
                await _next(context);
                stopwatch.Stop();


               _logger.LogInformation(
              "Requisição finalizada: {Metodo} {Caminho} respondeu {StatusCode} em {TempoDecorrido}ms",
              context.Request.Method,
              context.Request.Path,
              context.Response.StatusCode,
              stopwatch.ElapsedMilliseconds);

            }
            catch (Exception ex)
            {

                stopwatch.Stop();

                _logger.LogError(ex,
                    context.Request.Method,
                   "Requisição finalizada: {Metodo} {Caminho} respondeu {StatusCode} em {TempoDecorrido}ms",
                   context.Request.Method,
                   context.Request.Path,
                   context.Response.StatusCode,
                   stopwatch.ElapsedMilliseconds);
            }


        }

    }
}
