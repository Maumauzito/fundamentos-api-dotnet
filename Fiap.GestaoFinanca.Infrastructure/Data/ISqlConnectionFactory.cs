using System.Data;

namespace Fiap.GestaoFinanca.Infrastructure.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();

    }
}
