using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

public class Faturamento : EntidadeBase<Faturamento>
{
    public DateTime DataInicio {get; set; }
    public DateTime? DataFim {get; set; }
    public DateTime? DataPagamento {get; set; }
    public int? ValorTotal {get; set; }
    public Ticket Ticket {get; set; }

    public Faturamento(DateTime dataInicio, Ticket ticket)
    {
        Id = Guid.NewGuid();
        DataInicio = DateTime.Now;
        DataFim = null;
        DataPagamento = null;
        ValorTotal = null;
        Ticket = Ticket;
    }

    public override void AtualizarRegistro(Faturamento registroEditado)
    {
        DataFim = registroEditado.DataFim;
        DataPagamento = registroEditado.DataPagamento;
        ValorTotal = registroEditado.ValorTotal;
        Ticket = registroEditado.Ticket;
    }
}