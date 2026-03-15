using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class ModuloConfiguration : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.ToTable("modulos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasMaxLength(50);

            // Índice único em Nome
            builder.HasIndex(e => e.Nome)
                   .IsUnique()
                   .HasDatabaseName("ix_modulo_nome");
        }

    }
}

