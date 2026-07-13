using System.Text.Json;

namespace Fiap.GestaoFinanca.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext contex) 
        {
            try 
            {    
                await _next(contex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro não tratado durante a requisição {Method} - {Path}",
                    contex.Request.Method,
                    contex.Request.Path);

                await EscreverRespostaErroAsync(contex);
            }
        
        }

        private static async Task EscreverRespostaErroAsync(HttpContext contex)
        {
            if (contex.Response.HasStarted)
            {
                return;
            }

            contex.Response.Clear();
            contex.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
            contex.Response.ContentType = "application/json";


            var resposta = new
            {
                erro = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.",
                statuscode = contex.Response.StatusCode,
                traceId = contex.TraceIdentifier
            };


            var json = JsonSerializer.Serialize(resposta, new JsonSerializerOptions 
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await contex.Response.WriteAsync(json);

        }
    }
}
