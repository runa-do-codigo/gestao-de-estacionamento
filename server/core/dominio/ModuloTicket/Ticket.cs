using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloTicket;

public class Ticket : EntidadeBase<Ticket>
{
    public DateTime DataEntrada { get; set; }
    public DateTime? DataSaida { get; set; }
    public Veiculo Veiculo { get; set; }
    public Guid FaturamentoId { get; set; }
    public Vaga Vaga { get; set; }

    public Ticket() { }
    public Ticket(Veiculo veiculo, Vaga vaga) : this()
    {
        Id = Guid.NewGuid();
        DataEntrada = DateTime.Now;
        DataSaida = null;
        Veiculo = veiculo;
        Vaga = vaga;
    }

    public void Saida()
    {
        DataSaida = DateTime.Now;
    }
    public override void AtualizarRegistro(Ticket registroEditado)
    {
        DataSaida = registroEditado.DataSaida;
        DataEntrada = registroEditado.DataEntrada;
        Veiculo = registroEditado.Veiculo;
        Vaga = registroEditado.Vaga;
    }
}