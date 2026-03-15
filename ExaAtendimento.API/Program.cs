using ExaAtendimento.API;
using ExaAtendimento.API.Filters;
using ExaAtendimento.API.Helpers;
using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Services;
using ExaAtendimento.Application.Settings;
using ExaAtendimento.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços utilizando extensão
builder.Services.AdicionarServicosDaAplicacao(builder.Configuration);

// Configuração dos Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// settings - Urls FrontEnd
builder.Services.Configure<FrontendUrlsSettings>(
        builder.Configuration.GetSection("FrontendUrls"));

// Configuração do JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

//app.UseHttpsRedirection();

app.UseCors("AllowAngular");

// tratamento global de erros
app.UseMiddleware<ExaAtendimento.API.Middlewares.ExceptionMiddleware>();

// padronizar todos os códigos de status não tratados
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    response.ContentType = "application/json";

    var result = JsonSerializer.Serialize(new
    {
        statusCode = response.StatusCode,
        message = ApiErrorHelper.GetMessageForStatusCode(response.StatusCode)
    });

    await response.WriteAsync(result);
});

// Autenticação e Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//rotinas de inicialização do sistema - criação do usuário admin
var adminSettings = builder.Configuration.GetSection("AdminSettings");
var userAdmin = adminSettings["Useradmin"];
var emailAdmin = adminSettings["Emailadmin"];
var passAdmin = adminSettings["Passadmin"];

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var usuarioService = services.GetRequiredService<UsuarioService>();
        var usuarioResponse = await usuarioService.ObterPorEmailAsync(emailAdmin);

        if (!usuarioResponse.Sucesso)
        {
            var novoAdmin = new UsuarioCriacaoDto
            {
                Nome = userAdmin,
                Email = emailAdmin,
                Perfil = PerfilUsuario.Administrador,
                Senha = passAdmin
            };

            await usuarioService.CriarAdminAsync(novoAdmin);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar usuário admin: {ex.Message}");
    }
}

app.Run();
