using Dapper;
using Fiap.GestaoFinanca.Application.Respositories;
using Fiap.GestaoFinanca.Domain.Entities;
using Fiap.GestaoFinanca.Infrastructure.Data;
using Fiap.GestaoFinanca.Infrastructure.Resilience;
using Fiap.GestaoFinanca.Infrastructure.Simulations;
using Microsoft.Extensions.Logging;


namespace Fiap.GestaoFinanca.Infrastructure.Respositories
{
    public class DespesaRepository : IDespesaRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly ILogger<DespesaRepository> _logger;
        private readonly DataBaseInstabilitySimulator _dataBaseInstabilitySimulator;

        public DespesaRepository(ISqlConnectionFactory connectionFactory,
                                ILogger<DespesaRepository> logger,
                                DataBaseInstabilitySimulator dataBaseInstabilitySimulator)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _dataBaseInstabilitySimulator = dataBaseInstabilitySimulator;
        }


        public async Task<IReadOnlyCollection<Despesa>> ListarAsync()
        {

            const string sql = """
                SELECT Id, 
                    Descricao, 
                    Valor, 
                    Data, 
                    Categoria, 
                    FormaPagamento, 
                    CriadoEm
                FROM Despesas
                ORDER BY Data DESC
                """;


            var policy = DataBaseResiliencePolicy.CreateCombinePolicy(_logger);

            return await policy.ExecuteAsync(async () => 
            {

                _logger.LogInformation("========= Consultando despesas no banco.==========");


                using var connetion = _connectionFactory.CreateConnection();
                var despesas = await connetion.QueryAsync<Despesa>(sql);

                return despesas.ToList();

            });

        }

        public async Task<Despesa?> ObterPorIdAsync(Guid id)
        {
            const string sql = """
                SELECT Id, 
                    Descricao, 
                    Valor, 
                    Data, 
                    Categoria, 
                    FormaPagamento, 
                    CriadoEm
                FROM Despesas
                WHERE Id = @Id
                """;

            using var connetion = _connectionFactory.CreateConnection();

            return await connetion.QuerySingleOrDefaultAsync<Despesa>(sql, new { Id = id });

        }

        public async Task AdcionarAsync(Despesa despesa)
        {
            const string sql = """
                INSERT INTO Despesas (
                    Id, 
                    Descricao,
                    Valor,
                    Data,
                    Categoria,
                    FormaPagamento,
                    CriadoEm)
                VALUES (@Id,
                        @Descricao,
                        @Valor,
                        @Data,
                        @Categoria,
                        @FormaPagamento,
                        @CriadoEm)
                """;

            using var connetion = _connectionFactory.CreateConnection();

            await connetion.ExecuteAsync(sql, despesa);

        }

        public async Task AtualizarAsync(Despesa despesa)
        {
            const string sql = """
                UPDATE Despesas
                SET Descricao = @Descricao,
                    Valor = @Valor,
                    Data = @Data,
                    Categoria = @Categoria,
                    FormaPagamento = @FormaPagamento
                WHERE Id = @Id
                """;

            using var connetion = _connectionFactory.CreateConnection();

            await connetion.ExecuteAsync(sql, despesa);

        }

        public Task RemoverAsync(Guid id)
        {
           const string sql = """
                DELETE FROM Despesas
                WHERE Id = @Id
                """;
            using var connetion = _connectionFactory.CreateConnection();
            return connetion.ExecuteAsync(sql, new { Id = id });
        }
    }
}
