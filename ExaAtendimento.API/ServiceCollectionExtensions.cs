using ExaAtendimento.Application.Interfaces.Utils;
using ExaAtendimento.Application.Mappings;
using ExaAtendimento.Application.Services;
using ExaAtendimento.Application.Services.Auth;
using ExaAtendimento.Application.Validators;
using ExaAtendimento.Domain.Interfaces;

using ExaAtendimento.InfraData.Data;
using ExaAtendimento.InfraData.Repositories;
using ExaAtendimento.InfraData.Utils;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Internal;


namespace ExaAtendimento.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AdicionarServicosDaAplicacao(this IServiceCollection services, IConfiguration configuration)
        {
            // Mapster
            services.AddMapster();
            MappingConfig.RegisterMappings(TypeAdapterConfig.GlobalSettings);

            // DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 11, 0))));
            services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Repositórios (OBS: Pacote: Scrutor faz automático)
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();
            services.AddScoped<ICaRepository, CaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IModuloRepository, ModuloRepository>();
            services.AddScoped<ISugestaoRepository, SugestaoRepository>();
            services.AddScoped<ITipoAtendimentoRepository, TipoAtendimentoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Serviços de Domínio (OBS: Pacote: Scrutor faz automático)
            services.AddScoped<AssuntoService>();
            services.AddScoped<AtendimentoService>();
            services.AddScoped<CaService>();
            services.AddScoped<ClienteService>();
            services.AddScoped<ModuloService>();
            services.AddScoped<SugestaoService>();
            services.AddScoped<TipoAtendimentoService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<DashboardService>();

            //outros recursos utilitários (envio de email)
            services.AddTransient<IEmailService, SmtpEmailServiceAsync>();

            // Validators (basta registrar um)
            services.AddValidatorsFromAssembly(typeof(ModuloDtoValidator).Assembly);

            return services;
        }
    }
}
