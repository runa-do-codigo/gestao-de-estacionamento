using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;

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

        builder.Property(x => x.Veiculo)
            .IsRequired();

        //builder.Property(x => x.Vaga)
            //.IsRequired();

        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}