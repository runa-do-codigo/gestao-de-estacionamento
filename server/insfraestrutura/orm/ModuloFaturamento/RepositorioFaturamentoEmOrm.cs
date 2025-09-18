using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloFaturamento;

public class RepositorioFaturamentoEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Faturamento>(contexto), IRepositorioFaturamento
{
    public override async Task<List<Faturamento>> SelecionarRegistrosAsync()
    {
        return await registros.Include(x => x.Ticket).ToListAsync();
    }

    public override async Task<Faturamento?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.Include(x => x.Ticket).FirstOrDefaultAsync(x => x.Id == idRegistro);
    }

    public Task<Faturamento> ObterTotalFatura(Guid idFatura)
    {
        throw new NotImplementedException();
    }
}