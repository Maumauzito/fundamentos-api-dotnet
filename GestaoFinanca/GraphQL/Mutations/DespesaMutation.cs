using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;

namespace Fiap.GestaoFinanca.Api.GraphQL.Mutations
{
    public class DespesaMutation
    {
        [GraphQLName("cadastrar")]
        public async Task<DespesaResponse> CadastrarDespesaAsync(
            DespesaRequest despesaRequest,
            [Service] IDespesaService despesaService)
        {
            return await despesaService.CriarAsync(despesaRequest);
        }


    }
}
