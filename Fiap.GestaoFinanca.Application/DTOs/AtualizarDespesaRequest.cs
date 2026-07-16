using System;
using System.Collections.Generic;
using System.Text;

namespace Fiap.GestaoFinanca.Application.DTOs
{
   public record AtualizarDespesaRequest
                    (string Descricao,
                    decimal Valor,
                    DateTime Data,
                    string Categoria,
                    string FormaPagamento);
}
