using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.InfraData.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("clientes");

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

            builder.Property(e => e.CaId)
                   .IsRequired();

            builder.Property(e => e.CaCompartilhadaId);

            // fk - Ca
            builder.HasOne(e => e.Ca)
                   .WithMany()
                   .HasForeignKey(e => e.CaId)
                   .HasConstraintName("fk_cliente_ca")
                   .OnDelete(DeleteBehavior.Restrict);

            // fk - CaCompartilhada
            builder.HasOne(e => e.CaCompartilhada)
                   .WithMany()
                   .HasForeignKey(e => e.CaCompartilhadaId)
                   .HasConstraintName("fk_cliente_cacompartilhada")
                   .OnDelete(DeleteBehavior.Restrict);

            // índices
            builder.HasIndex(e => e.Nome)
            // não criar único, pode haver repetição de nomes
            //       .IsUnique()
                   .HasDatabaseName("ix_cliente_nome");
        }
    }
}
