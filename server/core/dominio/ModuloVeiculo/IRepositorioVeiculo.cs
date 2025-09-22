using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
public interface IRepositorioVeiculo : IRepositorio<Veiculo>
{
    public Task<Veiculo> CadastrarObservacao(Guid idVeiculo, string observacao);
}