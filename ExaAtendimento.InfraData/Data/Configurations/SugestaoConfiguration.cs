using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class SugestaoConfiguration : IEntityTypeConfiguration<Sugestao>
    {
        public void Configure(EntityTypeBuilder<Sugestao> builder)
        {
            builder.ToTable("sugestoes");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.Descricao)
                   .IsRequired()
                   .HasMaxLength(50);

            // índice
            builder.HasIndex(e => e.Descricao)
                   .IsUnique()
                   .HasDatabaseName("ix_sugestao_descricao");
        }
    }
}
