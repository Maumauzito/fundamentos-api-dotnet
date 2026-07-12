using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;
using Fiap.GestaoFinanca.Application.Mappings;
using Fiap.GestaoFinanca.Domain.Entities;

namespace Fiap.GestaoFinanca.Application.Services
{
    public class DespesaService : IDespesaService
    {

        private static readonly List<Despesa> Despesas = [];
        public DespesaResponse Criar(DespesaRequest request)
        {
            var despesa = new Despesa(
                request.Descricao,
                request.Valor,
                request.Data,
                request.Categoria,
                request.FormaPagamento
                );

            Despesas.Add(despesa);

            return despesa.ToResponse();

        }

        public IReadOnlyCollection<DespesaResponse> Listar()
        {
            return Despesas.OrderByDescending(d => d.Data)
                .Select(d => d.ToResponse())
                .ToList();
        }

        public DespesaResponse? ObterPorId(Guid id)
        {
            var despesa = Despesas.FirstOrDefault(d => d.Id == id);
            return despesa?.ToResponse();
        }
    }
}
