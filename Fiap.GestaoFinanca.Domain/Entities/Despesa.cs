namespace Fiap.GestaoFinanca.Domain.Entities
{
    public class Despesa
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; } = string.Empty;
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }
        public string Categoria { get; private set; } = string.Empty;
        public string FormaPagamento { get; private set; } = string.Empty;
        public DateTime CriadoEm { get; private set; }


        public Despesa(
            string descricao,
            decimal valor,
            DateTime data,
            string categoria,
            string formaPagamento)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Valor = valor;
            Data = data;
            Categoria = categoria;
            FormaPagamento = formaPagamento;
            CriadoEm = DateTime.UtcNow;
        }
        
        public void Atualizar(
            string descricao,
            decimal valor,
            DateTime data,
            string categoria,
            string formaPagamento)
        {
            Descricao = descricao;
            Valor = valor;
            Data = data;
            Categoria = categoria;
            FormaPagamento = formaPagamento;
        }

        private Despesa()
        {
            Descricao = string.Empty;
            Categoria = string.Empty;
            FormaPagamento = string.Empty;
        }
    }
}
