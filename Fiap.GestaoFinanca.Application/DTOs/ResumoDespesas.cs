namespace Fiap.GestaoFinanca.Application.DTOs
{
    public record ResumoDespesas
    {
        public int Quantidade { get; init; }
        public decimal Valor { get; init; }
        public string Mensagem { get; init; } = string.Empty;
    }
}

