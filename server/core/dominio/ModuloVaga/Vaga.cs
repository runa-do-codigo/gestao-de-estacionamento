using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVaga;

public class Vaga : EntidadeBase<Vaga>
{
    public int Numero { get; set; }
    public bool EstaOcupada { get; set; }
    public Guid TicketId { get; set; }
    public Vaga() { }
    public Vaga(int numero) : this()
    {
        Numero = numero;
        EstaOcupada = false;
    }

    public void OcuparVaga()
    {
        EstaOcupada = true;
    }
    public override void AtualizarRegistro(Vaga registroEditado)
    {
        Numero = registroEditado.Numero;
        EstaOcupada = registroEditado.EstaOcupada;
    }
}
