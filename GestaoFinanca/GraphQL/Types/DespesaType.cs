using Fiap.GestaoFinanca.Application.DTOs;

namespace Fiap.GestaoFinanca.Api.GraphQL.Types
{
    public class DespesaType : ObjectType<DespesaResponse>
    {
        protected override void Configure(IObjectTypeDescriptor<DespesaResponse> descriptor)
        {
            descriptor.Description("Representa um despesa cadastrada no sistema");

            descriptor.Field(d => d.Id).Description("Identificador único da despesa");

            descriptor.Field(d => d.Descricao).Description("Descrição informada para a despesa.");

            descriptor.Field(d => d.Valor).Description("Valor monetário da despesa.");

            descriptor.Field(d => d.Data).Description("Data em que a despesa ocorreu.");

            descriptor.Field(d => d.Categoria).Description("Categoria associada à despesa.");

            descriptor.Field(d => d.FormaPagamento).Description("Forma de pagamento utilizada para a despesa.");

        }
    }
}
