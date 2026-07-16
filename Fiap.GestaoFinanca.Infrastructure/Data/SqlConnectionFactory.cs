using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Fiap.GestaoFinanca.Infrastructure.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
             _connectionString = configuration.GetConnectionString("SqlServer")
                ?? throw new InvalidOperationException("Conexão não encontrada");
        }

        public IDbConnection CreateConnection()
        {
           return new SqlConnection(_connectionString);
        }
    }
}
