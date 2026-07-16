using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;
using Fiap.GestaoFinanca.Application.Mappings;
using Fiap.GestaoFinanca.Application.Respositories;
using Fiap.GestaoFinanca.Domain.Entities;

namespace Fiap.GestaoFinanca.Application.Services
{
    public class DespesaService : IDespesaService
    {

        private readonly IDespesaRepository _despesaRepository;

        public DespesaService(IDespesaRepository despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }

        public async Task<IReadOnlyCollection<DespesaResponse>> ListarAsync()
        {

            var despesas = await _despesaRepository.ListarAsync();

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
            var despesa = new Despesa(
                request.Descricao,
                request.Valor,
                request.Data,
                request.Categoria,
                request.FormaPagamento
                );

            await _despesaRepository.AdcionarAsync(despesa);

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
