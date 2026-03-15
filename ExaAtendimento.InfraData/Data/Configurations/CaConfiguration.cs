using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class CaConfiguration : IEntityTypeConfiguration<Ca>
    {
        public void Configure(EntityTypeBuilder<Ca> builder)
        {
            builder.ToTable("cas");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Cidade)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Uf)
                   .IsRequired()
                   .HasMaxLength(2);

            // índices
            builder.HasIndex(e => e.Nome)
            // não criar único, pode haver repetição de nomes
            //       .IsUnique()
                   .HasDatabaseName("ix_ca_nome");

        }
    }
}