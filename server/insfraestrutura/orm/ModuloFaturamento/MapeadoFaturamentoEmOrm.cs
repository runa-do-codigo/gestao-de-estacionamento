using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;

public class MapeadoFaturamentoEmOrm : IEntityTypeConfiguration<Faturamento>
{
    public void Configure(EntityTypeBuilder<Faturamento> builder)
    {
        builder.Property(x => x.Id)
           .ValueGeneratedNever()
           .IsRequired();
        builder.Property(x => x.DataPagamento)
            .IsRequired(false);

        builder.Property(x => x.ValorTotal)
            .IsRequired(false);

        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}