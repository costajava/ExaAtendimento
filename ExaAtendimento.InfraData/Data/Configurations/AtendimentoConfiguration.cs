using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class AtendimentoConfiguration : IEntityTypeConfiguration<Atendimento>
    {
        public void Configure(EntityTypeBuilder<Atendimento> builder)
        {
            builder.ToTable("atendimentos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.DataAtendimento)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(e => e.DataRegistro).IsRequired();

            builder.Property(e => e.AtendimentoConcluido)
                   .IsRequired();

            builder.Property(e => e.Observacoes)
                   .HasMaxLength(500);

            builder.Property(e => e.CobrarCliente).IsRequired();

            builder.Property(e => e.HoraInicial)
                   .HasColumnType("time")
                   .IsRequired();

            builder.Property(e => e.HoraFinal)
                   .HasColumnType("time");

            builder.Property(e => e.Encerrado).IsRequired();

            builder.Property(e => e.Contato)
                   .HasMaxLength(50);

            builder.Property(e => e.NumTipoAtendimento)
                   .HasMaxLength(10);

            builder.Property(e => e.AnoRegistro).IsRequired();

            builder.Property(e => e.ClienteCodigo);

            builder.Property(e => e.CaId).IsRequired();

            builder.Property(e => e.ClienteId);

            builder.Property(e => e.SugestaoId).IsRequired();

            builder.Property(e => e.ModuloId).IsRequired();

            builder.Property(e => e.AssuntoId).IsRequired();

            builder.Property(e => e.UsuarioId).IsRequired();

            builder.Property(e => e.TipoAtendimentoId).IsRequired();

            // fk - Ca
            builder.HasOne(e => e.Ca)
                   .WithMany()
                   .HasForeignKey(e => e.CaId)
                   .HasConstraintName("fk_atendimento_ca")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - Cliente
            builder.HasOne(e => e.Cliente)
                   .WithMany()
                   .HasForeignKey(e => e.ClienteId)
                   .HasConstraintName("fk_atendimento_cliente")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - Modulo
            builder.HasOne(e => e.Modulo)
                   .WithMany()
                   .HasForeignKey(e => e.ModuloId)
                   .HasConstraintName("fk_atendimento_modulo")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - Sugestao
            builder.HasOne(e => e.Sugestao)
                   .WithMany()
                   .HasForeignKey(e => e.SugestaoId)
                   .HasConstraintName("fk_atendimento_sugestao")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - Assunto
            builder.HasOne(e => e.Assunto)
                   .WithMany()
                   .HasForeignKey(e => e.AssuntoId)
                   .HasConstraintName("fk_atendimento_assunto")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - Usuario
            builder.HasOne(e => e.Usuario)
                   .WithMany()
                   .HasForeignKey(e => e.UsuarioId)
                   .HasConstraintName("fk_atendimento_usuario")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - TipoAtendimento
            builder.HasOne(e => e.TipoAtendimento)
                   .WithMany()
                   .HasForeignKey(e => e.TipoAtendimentoId)
                   .HasConstraintName("fk_atendimento_tipoatendimento")
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
