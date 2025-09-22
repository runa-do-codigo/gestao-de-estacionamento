using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloVeiculo;
public class MapeadorVeiculoOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Placa)
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.Modelo)
            .IsRequired();

        builder.Property(x => x.Cor)
            .IsRequired();

        builder.Property(x => x.Observacao)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.HasOne(x => x.Hospede)
            .WithMany();
    }
}