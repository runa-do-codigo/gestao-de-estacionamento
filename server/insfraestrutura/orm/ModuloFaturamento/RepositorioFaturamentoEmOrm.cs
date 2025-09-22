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

    public async Task<int?> ObterTotalFatura(Guid idFatura)
    {
        var faturamento = await contexto.Faturamentos
            .Include(f => f.Ticket)
            .FirstOrDefaultAsync(f => f.Id == idFatura);


        TimeSpan permanencia = faturamento.Ticket.DataSaida.Value - faturamento.Ticket.DataEntrada;

        const int precoPorHora = 10;
        int horas = (int)Math.Ceiling(permanencia.TotalHours);

        faturamento.ValorTotal = horas * precoPorHora;

        // Atualiza no banco
        contexto.Faturamentos.Update(faturamento);
        await contexto.SaveChangesAsync();

        return faturamento.ValorTotal; // int
    }
}