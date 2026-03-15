using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class AssuntoConfiguration : IEntityTypeConfiguration<Assunto>
    {
        public void Configure(EntityTypeBuilder<Assunto> builder)
        {
            builder.ToTable("assuntos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.TipoAssunto)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.ModuloId)
                   .IsRequired();

            // fk Modulo
            builder.HasOne(e => e.Modulo)
                   .WithMany()
                   .HasForeignKey(e => e.ModuloId)
                   .HasConstraintName("fk_assunto_modulo")
                   .OnDelete(DeleteBehavior.Restrict); 

            // índice
            builder.HasIndex(e => e.TipoAssunto)
                   .IsUnique()
                   .HasDatabaseName("ix_assunto_tipoAssunto");
        }
    }
}
