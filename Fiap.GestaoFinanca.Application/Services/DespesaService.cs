using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;
using Fiap.GestaoFinanca.Application.Mappings;
using Fiap.GestaoFinanca.Application.Respositories;
using Fiap.GestaoFinanca.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Fiap.GestaoFinanca.Application.Services
{
    public class DespesaService : IDespesaService
    {

        private readonly IDespesaRepository _despesaRepository;
        private readonly ILogger<DespesaService> _logger;

        public DespesaService(IDespesaRepository despesaRepository, ILogger<DespesaService> logger)
        {
            _despesaRepository = despesaRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<DespesaResponse>> ListarAsync()
        {

            _logger.LogInformation("Iniciando listagem de despesas");

            var despesas = await _despesaRepository.ListarAsync();

            _logger.LogInformation("Finalizando listagem de despesas");

            return despesas
                    .Select(despesa => despesa.ToResponse())
                    .ToList();
        }

        public async Task<DespesaResponse?> ObterPorIdAsync(Guid id)
        {
            var despesa = await _despesaRepository.ObterPorIdAsync(id);
            
            return despesa?.ToResponse();
        }


        public async Task<DespesaResponse> CriarAsync(DespesaRequest request)
        {

            _logger.LogInformation(
                    "Iniciando criação de despesa. Descricao: {Descricao}, Valor: {Valor}, Categoria: {Categoria}",
                    request.Descricao,
                    request.Valor,
                    request.Categoria);


            var despesa = new Despesa(
                request.Descricao,
                request.Valor,
                request.Data,
                request.Categoria,
                request.FormaPagamento
                );

            await _despesaRepository.AdcionarAsync(despesa);

            _logger.LogInformation("Despesa criada com sucesso. Id: {DespesaId}", despesa.Id);

            return despesa.ToResponse();

        }


        public async Task<bool> AtualizarAsync(Guid id, AtualizarDespesaRequest request)
        {

            var despesa = await _despesaRepository.ObterPorIdAsync(id);

            if (despesa is null)
                return false;

            despesa.Atualizar(
                request.Descricao,
                request.Valor,
                request.Data,
                request.Categoria,
                request.FormaPagamento
                );

            await _despesaRepository.AtualizarAsync(despesa);
            return true;
        }

        public async Task<bool> ExcluirAsync(Guid id)
        {
            var despesa = await _despesaRepository.ObterPorIdAsync(id);

            if (despesa is null)
                return false;

            await _despesaRepository.RemoverAsync(id);

            return true;
        }

    }
}
