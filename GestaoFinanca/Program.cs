using Fiap.GestaoFinanca.Api.Endpoints;
using Fiap.GestaoFinanca.Api.Extensions;
using Fiap.GestaoFinanca.Application.DependencyInjection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Registra os controllers da aplicação no container de dependências.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddApplication();

// Registra a documentação OpenAPI, útil para testar e documentar endpoints.
builder.Services.AddOpenApi();


var app = builder.Build();


// Em ambiente de desenvolvimento, expõe o documento OpenAPI.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redireciona requisições HTTP para HTTPS.
app.UseHttpsRedirection();


app.UseApiMiddlewares();

app.MapGet("/api/status",() => 
{
    return Results.Ok(new
    {
        status = "API em execução",
        application = "Gestão Financeira",
        Framework = ".NET 10.0",
        ExecutadoEm = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
    });
});

// Prepara o middleware de autorização.
app.UseAuthorization();

app.MapdespesaEndpoints();

// Mapeia os controllers como endpoints HTTP.
app.MapControllers();


app.MapGet("/api/teste-erro", () =>
 {
     throw new Exception("Erro de teste para o middleware de tratamento de exceções.");
 });



app.Run();
