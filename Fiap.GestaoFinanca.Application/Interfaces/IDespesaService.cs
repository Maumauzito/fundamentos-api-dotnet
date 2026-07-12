using Fiap.GestaoFinanca.Application.DTOs;

namespace Fiap.GestaoFinanca.Application.Interfaces
{
    public interface IDespesaService
    {
        IReadOnlyCollection<DespesaResponse> Listar();
        DespesaResponse? ObterPorId(Guid id);
        DespesaResponse Criar(DespesaRequest request);
    }
}
