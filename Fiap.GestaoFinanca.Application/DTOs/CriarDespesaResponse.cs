namespace Fiap.GestaoFinanca.Application.DTOs
{
    public record DespesaResponse(
        Guid Id,
        string Descricao,
        decimal Valor,
        DateTime Data,
        string Categoria,
        string FormaPagamento,
        DateTime CriadoEm,
        string? Observacao = null);

}
