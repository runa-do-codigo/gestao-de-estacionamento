using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;

public class MapeadoTicketEmOrm : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(x => x.Id)
           .ValueGeneratedNever()
           .IsRequired();

        builder.Property(x => x.DataEntrada)
            .IsRequired(false);

        builder.Property(x => x.DataSaida)
            .IsRequired(false);

        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}