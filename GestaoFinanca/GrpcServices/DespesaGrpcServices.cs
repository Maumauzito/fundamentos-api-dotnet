using Fiap.GestaoFinanca.Application.Interfaces;
using GestaoFinanca.API.Protos;
using Grpc.Core;

namespace Fiap.GestaoFinanca.Api.GrpcServices
{
    public class DespesaGrpcServices : DespesasGrpc.DespesasGrpcBase
    {
        private readonly ILogger<DespesaGrpcServices> _logger;
        private readonly IDespesaService _despesaService;

        public DespesaGrpcServices(ILogger<DespesaGrpcServices> logger, IDespesaService despesaService)
        {
            _logger = logger;
            _despesaService = despesaService;
        }



        public override async Task<ResumoDespesasResponse> ObterResumoDespesas(ResumoDespesasRequest request, ServerCallContext context)
        {

            _logger.LogInformation("ObterResumoDespesas called with UsuarioId: {UsuarioId}", request.UsuarioId);

            var resumo = await _despesaService.ObterResumoAsync(request.UsuarioId);

            return new ResumoDespesasResponse
            {
                Quantidade = resumo.Quantidade,
                ValorTotal = (double)resumo.Valor,
                Mensagem = resumo.Mensagem
            };
        }
    }
}
