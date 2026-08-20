using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;

namespace Fiap.GestaoFinanca.Api.GraphQL.Queries
{
    public class DespesaQuery
    {
        [GraphQLName("despesas")]
        public async Task<IEnumerable<DespesaResponse>> ObterDespesasAsync(
            [Service] IDespesaService despesaService)
        {
            return await despesaService.ListarAsync();
        }

        [GraphQLName("despesasPorId")]
        public async Task<DespesaResponse?> ObterPorId(Guid id, [Service] IDespesaService despesaService)
        {
            return await despesaService.ObterPorIdAsync(id);
        }

    }
}
