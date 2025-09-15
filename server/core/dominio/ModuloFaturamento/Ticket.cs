using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

public class Ticket : EntidadeBase<Ticket>
{
    public DateTime DataEntrada { get; set; }
    public DateTime? DataSaida { get; set; }
    public Faturamento Faturamento { get; set; }

    public Ticket(DateTime dataEntrada, Faturamento faturamento)
    {
        Id = Guid.NewGuid();
        DataEntrada = DateTime.Now;
        DataSaida = null;
        Faturamento = faturamento;
    }

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        DataSaida = registroEditado.DataSaida;
        Faturamento = registroEditado.Faturamento;
    }
}