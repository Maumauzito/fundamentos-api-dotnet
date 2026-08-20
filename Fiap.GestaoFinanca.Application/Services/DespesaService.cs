using Fiap.GestaoFinanca.Application.DTOs;
using Fiap.GestaoFinanca.Application.Interfaces;
using Fiap.GestaoFinanca.Application.Mappings;
using Fiap.GestaoFinanca.Application.Respositories;
using Fiap.GestaoFinanca.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Fiap.GestaoFinanca.Application.Caching;

namespace Fiap.GestaoFinanca.Application.Services
{
    public class DespesaService : IDespesaService
    {

        private readonly IDespesaRepository _despesaRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<DespesaService> _logger;

        public DespesaService(IDespesaRepository despesaRepository, IMemoryCache memoryCache, ILogger<DespesaService> logger)
        {
            _despesaRepository = despesaRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<DespesaResponse>> ListarAsync()
        {



            if (_memoryCache.TryGetValue(CacheKeys.TodasDespesasCache, out IEnumerable<DespesaResponse>? despesasEmCache))
            {
                _logger.LogInformation("Despesas retornadas a partir do cache");
                return (IReadOnlyCollection<DespesaResponse>)despesasEmCache;
            }



            _logger.LogInformation("Iniciando listagem de despesas");

            var despesas = await _despesaRepository.ListarAsync();

            _logger.LogInformation("Finalizando listagem de despesas");

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                SlidingExpiration = TimeSpan.FromSeconds(30),
            };

            var resultado = despesas
                    .Select(despesa => despesa.ToResponse())
                    .ToList();


            _memoryCache.Set(CacheKeys.TodasDespesasCache, resultado, cacheOptions);

            _logger.LogInformation("Despesas armazendas em chache : {TodasDespesasCache}", CacheKeys.TodasDespesasCache);

            return resultado;
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

            _memoryCache.Remove(CacheKeys.TodasDespesasCache);

            _logger.LogInformation("Cache de despesas invalidado. Chave {Chave}", CacheKeys.TodasDespesasCache);

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


        public async Task<ResumoDespesas> ObterResumoAsync(string id) 
        {
            var despesas = await _despesaRepository.ListarAsync();

            var lista = despesas.ToList();

            var quantidade = lista.Count();
            var valorTotal = lista.Sum(d => d.Valor);

            var mesagem = quantidade == 0 ? "Nenhuma despesa cadastrada." : $"Total de despesas: {quantidade}, Valor total: {valorTotal:C}";

            return new ResumoDespesas
            {
                Quantidade = quantidade,
                Valor = valorTotal,
                Mensagem = mesagem
            };
        }

    }
}
