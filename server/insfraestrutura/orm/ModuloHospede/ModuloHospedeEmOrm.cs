using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloHospede;
public class MapeadorHospedeEmOrm : IEntityTypeConfiguration<Hospede>
{
    public void Configure(EntityTypeBuilder<Hospede> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired();

        builder.Property(x => x.CPF)
            .IsRequired();
    }
}