using Fiap.GestaoFinanca.Application.DTOs;

namespace Fiap.GestaoFinanca.Application.Interfaces
{
    public interface IDespesaService
    {
        Task<IReadOnlyCollection<DespesaResponse>> ListarAsync();
        Task<DespesaResponse?> ObterPorIdAsync(Guid id);
        Task<DespesaResponse> CriarAsync(DespesaRequest request);
        Task<bool> AtualizarAsync(Guid id, AtualizarDespesaRequest request);
        Task<bool> ExcluirAsync(Guid id);
        Task<ResumoDespesas> ObterResumoAsync(string id);
    }
}
