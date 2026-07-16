using Fiap.GestaoFinanca.Api.Endpoints;
using Fiap.GestaoFinanca.Api.Extensions;
using Fiap.GestaoFinanca.Application.DependencyInjection;
using System.Text.Json.Serialization;
using Fiap.GestaoFinanca.Infrastructure.DependencyInjection;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);


// Registra os controllers da aplicação no container de dependências.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();

//Add documentação do Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gestão Financeira API",
        Version = "v1",
        Description = "API para gerenciamento de despesas e receitas.",
        Contact = new OpenApiContact
        {
            Name = "Equipe de Academica",
            Email = "contato@fiap.com.br"
        }
    });
});


var app = builder.Build();


// Em ambiente de desenvolvimento, expõe o documento OpenAPI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestão Financeira API v1");
        options.RoutePrefix = "swagger"; // Define a raiz do Swagger UI como a rota padrão
    });
}

// Redireciona requisições HTTP para HTTPS.
app.UseHttpsRedirection();


app.UseApiMiddlewares();

app.MapGet("/api/status", () =>
{
    return Results.Ok(new
    {
        status = "API em execução",
        application = "Gestão Financeira",
        Framework = ".NET 10.0",
        ExecutadoEm = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
    });
});


app.MapGet("/", () => Results.Ok(
    new
    {
        Aplicacao = "Gestão Financeira",
        Versao = "1.0.0",
        Documentacao = "/swagger",
    }));

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
