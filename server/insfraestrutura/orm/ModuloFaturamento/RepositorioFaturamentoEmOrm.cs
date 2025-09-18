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

    public async Task<Faturamento> ObterTotalFatura(Guid idFatura)
    {
        // Busca a fatura pelo Id
        var faturamento = await contexto.Faturamentos
            .Include(f => f.Ticket) // inclui o ticket relacionado
            .FirstOrDefaultAsync(f => f.Id == idFatura);

        if (faturamento == null)
            throw new Exception("Fatura não encontrada.");

        if (faturamento.Ticket?.DataSaida == null)
            throw new Exception("Ticket ainda não finalizado (DataSaida não definida).");

        // Calcula tempo total
        TimeSpan permanencia = faturamento.Ticket.DataSaida.Value - faturamento.Ticket.DataEntrada;

        // Exemplo: valor fixo por hora
        const int precoPorHora = 10;

        // Arredonda para cima (cobrar hora cheia)
        int horas = (int)Math.Ceiling(permanencia.TotalHours);

        faturamento.ValorTotal = horas * precoPorHora;

        return faturamento;
    }

}