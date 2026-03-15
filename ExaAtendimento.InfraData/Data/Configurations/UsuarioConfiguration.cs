using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;
using System.Net.Mail;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuarios");
            
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Perfil)
                   .IsRequired();

            builder.Property(e => e.Senha)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.OldSenha)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.ModuloId);

            builder.Property(e => e.ResetSenhaToken)
                   .HasMaxLength(64);

            builder.Property(e => e.ResetSenhaExpiracao)
                   .HasColumnType("DATETIME");

            // fk Usuario - Modulo
            builder.HasOne(e => e.Modulo)
                   .WithMany()
                   .HasForeignKey(e => e.ModuloId)
                   .HasConstraintName("fk_usuario_modulo")
                   .OnDelete(DeleteBehavior.Restrict);

            // índices
            builder.HasIndex(e => e.Nome)
                   .IsUnique()
                   .HasDatabaseName("ix_usuario_nome");

            builder.HasIndex(e => e.Email)
                   .IsUnique()
                   .HasDatabaseName("ix_usuario_email");

            builder.HasIndex(e => e.ResetSenhaToken)
                   .IsUnique()
                   .HasDatabaseName("ix_usuario_token");

        }

    }
}
