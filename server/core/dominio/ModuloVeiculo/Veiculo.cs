using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
public class Veiculo : EntidadeBase<Veiculo>
{
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public Hospede Hospede { get; set; }

    public Veiculo() { }

    public Veiculo(string placa, string modelo, string cor, Hospede hospede) : this()
    {
        Placa = placa;
        Modelo = modelo;
        Cor = cor;
        Hospede = hospede;
    }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        Placa = registroEditado.Placa;
        Modelo = registroEditado.Modelo;
        Cor = registroEditado.Cor;
        Hospede = registroEditado.Hospede;
    }
}