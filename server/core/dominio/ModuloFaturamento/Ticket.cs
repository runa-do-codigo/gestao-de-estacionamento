using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

public class Ticket : EntidadeBase<Ticket>
{
    public DateTime DataEntrada { get; set; }
    public DateTime? DataSaida { get; set; }

    public Ticket(DateTime dataEntrada, Faturamento faturamento)
    {
        Id = Guid.NewGuid();
        DataEntrada = DateTime.Now;
        DataSaida = null;
    }

    public override void AtualizarRegistro(Ticket registroEditado)
    {
        DataSaida = registroEditado.DataSaida;
    }
}