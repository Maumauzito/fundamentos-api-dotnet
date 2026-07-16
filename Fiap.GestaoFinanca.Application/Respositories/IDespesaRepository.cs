using Fiap.GestaoFinanca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fiap.GestaoFinanca.Application.Respositories
{
    public interface IDespesaRepository
    {

        Task<IReadOnlyCollection<Despesa>> ListarAsync();
        Task<Despesa?> ObterPorIdAsync(Guid id);
        Task AdcionarAsync(Despesa despesa);
        Task AtualizarAsync(Despesa despesa);
        Task RemoverAsync(Guid id);

    }
}
