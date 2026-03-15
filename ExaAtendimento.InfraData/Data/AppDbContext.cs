using ExaAtendimento.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExaAtendimento.InfraData.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public virtual DbSet<Modulo> Modulos { get; set; }
        public virtual DbSet<Assunto> Assuntos { get; set; }
        public virtual DbSet<Sugestao> Sugestoes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<TipoAtendimento> TipoAtendimentos { get; set; }
        public virtual DbSet<Ca> Cas { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Atendimento> Atendimentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder
            //    .UseCollation("latin1_swedish_ci")
            //    .HasCharSet("latin1");

            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            // Aplica configurações das entidades da mesma assembly automaticamente
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
