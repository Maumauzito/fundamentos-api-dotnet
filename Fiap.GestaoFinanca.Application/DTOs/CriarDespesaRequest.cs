namespace Fiap.GestaoFinanca.Application.DTOs
{
    public record DespesaRequest(

        string Descricao,
        decimal Valor,
        DateTime Data,
        string Categoria,
        string FormaPagamento);
}
