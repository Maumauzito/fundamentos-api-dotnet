using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;

namespace Fiap.GestaoFinanca.Api.Endpoints
{
    public static class DespesaEndpoints
    {
        public static RouteGroupBuilder MapdespesaEndpoints(this IEndpointRouteBuilder app)
        {
            
            var group = app.MapGroup("/api/despesa-minimal")
                .WithTags("Despesas - Minimal API");

            
            
            group.MapGet("/", async  (IDespesaService despesaService, ILoggerFactory loggerFactory) =>
            {

                var logger = loggerFactory.CreateLogger("DespesaEndpoint");

                logger.LogInformation("Endpoint de listagem de despesas acionado");
                logger.LogDebug("Só aparece em DEBUG");

                var despesas =  await despesaService.ListarAsync();
                return Results.Ok(despesas);
            })
            .WithName("ListarDespesasMinimal")
            .WithSummary("Lista de todas as despesas")
            .WithDescription("Retorna dados de despesas cadastradas no SQL SERVER")
            .Produces<IEnumerable<DespesaResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/{id:guid}", async (Guid id, IDespesaService despesaService) => 
            {
                var despesas = await despesaService.ObterPorIdAsync(id);
                
                return despesas is null
                    ? Results.NotFound(new {Mensagem = "despesa não encontrada." })
                    : Results.Ok(despesas);
            })
                .WithName("ObterDespesaPorIdMinimal")
                .WithSummary("Busca uma despesa especifica com base em um ID")
                .WithDescription("Retorna dados de despesas especifica com base em um identificador unico")
                .Produces<IEnumerable<DespesaResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);


            group.MapPost("/", async (DespesaRequest request, IDespesaService despesaService) =>
            {
                var response = await despesaService.CriarAsync(request);
                return Results.Created($"/api/despesa-minimal/{response.Id}", response);
            } )
                .WithName("CriarDespesaMinimal")
                .WithSummary("Criação de despesa")
                .WithDescription("Recebe os dados de uma despesa e realiza o cadastro em banco de Dados")
                .Produces<IEnumerable<DespesaResponse>>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return group;

        }

    }
}
