using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
public class Hospede : EntidadeBase<Hospede>
{
    public string Nome { get; set; }
    public string CPF { get; set; }

    public Hospede() { }

    public Hospede(string nome, string cPF) : this()
    {
        Nome = nome;
        CPF = cPF;
    }
    public override void AtualizarRegistro(Hospede registroEditado)
    {
        Nome = registroEditado.Nome;
    }
}