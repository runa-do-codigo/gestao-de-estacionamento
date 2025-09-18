using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
public class Veiculo : EntidadeBase<Veiculo>
{
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }

    public Veiculo() { }

    public Veiculo(string placa, string modelo, string cor) : this()
    {
        Placa = placa;
        Modelo = modelo;
        Cor = cor;
    }

    public override void AtualizarRegistro(Veiculo registroEditado)
    {
        Placa = registroEditado.Placa;
        Modelo = registroEditado.Modelo;
        Cor = registroEditado.Cor;
    }
}