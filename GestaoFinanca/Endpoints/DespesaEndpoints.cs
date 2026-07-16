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

            
            
            group.MapGet("/", (IDespesaService despesaService) =>
            {
                var despesas = despesaService.ListarAsync();
                return Results.Ok(despesas);
            })
            .WithName("ListarDespesasMinimal");

            group.MapGet("/{id:guid}", async (Guid id, IDespesaService despesaService) => 
            {
                var despesas = await despesaService.ObterPorIdAsync(id);
                
                return despesas is null
                    ? Results.NotFound(new {Mensagem = "despesa não encontrada." })
                    : Results.Ok(despesas);
            })
                .WithName("ObterDespesaPorIdMinimal");
        
            
            group.MapPost("/",(DespesaRequest request, IDespesaService despesaService) =>
            {
                var response = despesaService.CriarAsync(request);
                return Results.Created($"/api/despesa-minimal/{response.Id}", response);
            } )
                .WithName("CriarDespesaMinimal");

            return group;

        }

    }
}
