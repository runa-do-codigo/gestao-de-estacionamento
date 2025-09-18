using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

public class Faturamento : EntidadeBase<Faturamento>
{
    public DateTime? DataPagamento {get; set; }
    public int? ValorTotal {get; set; }
    public Ticket Ticket {get; set; }

    public Faturamento()  { }
    public Faturamento(DateTime? dataPagamento, int? valorTotal, Ticket ticket) : this()
    {
        Id = Guid.NewGuid();
        DataPagamento = dataPagamento;
        ValorTotal = valorTotal;
        Ticket = ticket;
    }

    public override void AtualizarRegistro(Faturamento registroEditado)
    {
        DataPagamento = registroEditado.DataPagamento;
        ValorTotal = registroEditado.ValorTotal;
        Ticket = registroEditado.Ticket;
    }
}