using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

public interface IRepositorioFaturamento : IRepositorio<Faturamento>
{
    public Task<int?> ObterTotalFatura(Guid idFatura);
};