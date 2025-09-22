using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEstacionamento.Infraestrutura.Orm.ModuloTicket;

public class RepositorioTicketEmOrm(AppDbContext contexto)
    : RepositorioBaseEmOrm<Ticket>(contexto), IRepositorioTicket
{
    public override async Task<List<Ticket>> SelecionarRegistrosAsync()
    {
        return await registros.Include(x => x.Veiculo).Include(x => x.Vaga).ToListAsync();
    }

    public override async Task<Ticket?> SelecionarRegistroPorIdAsync(Guid idRegistro)
    {
        return await registros.Include(x => x.Veiculo).Include(x => x.Vaga).FirstOrDefaultAsync(x => x.Id == idRegistro);
    }
}