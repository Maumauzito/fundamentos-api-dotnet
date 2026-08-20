using Fiap.GestaoFinanca.Api.Endpoints;
using Fiap.GestaoFinanca.Api.Extensions;
using Fiap.GestaoFinanca.Api.GraphQL.Mutations;
using Fiap.GestaoFinanca.Api.GraphQL.Queries;
using Fiap.GestaoFinanca.Api.GrpcServices;
using Fiap.GestaoFinanca.Application.DependencyInjection;
using Fiap.GestaoFinanca.Infrastructure.Authentication;
using Fiap.GestaoFinanca.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;
using Fiap.GestaoFinanca.Api.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);


// Registra os controllers da aplicação no container de dependências.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddGrpc();

builder.Services.AddGraphQLServer()
    .AddQueryType<DespesaQuery>()
    .AddMutationType<DespesaMutation>()
    .AddType<DespesaType>();

builder.Services.AddMemoryCache();



var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>() ?? throw new InvalidOperationException("JwtSettings não encontradao");


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


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
            Name = "Equipe Academica",
            Email = "contato@fiap.com.br"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(document =>
    new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
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

app.UseAuthentication();
app.UseAuthorization();

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
    })).RequireAuthorization();

// Prepara o middleware de autorização.
app.UseAuthorization();

app.MapGrpcService<DespesaGrpcServices>();

app.MapGraphQL("/graphql");

app.MapAuthEndpoints();
app.MapdespesaEndpoints();
app.MapResilienceEndpoints();

// Mapeia os controllers como endpoints HTTP.
app.MapControllers();


app.MapGet("/api/teste-erro", () =>
 {
     throw new Exception("Erro de teste para o middleware de tratamento de exceções.");
 });


app.Run();
