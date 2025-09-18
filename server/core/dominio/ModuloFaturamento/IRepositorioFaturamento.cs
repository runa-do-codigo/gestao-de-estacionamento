using GestaoDeEstacionamento.Core.Dominio.Compartilhado;

namespace GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;

public interface IRepositorioFaturamento : IRepositorio<Faturamento>
{
    public Task<Faturamento> ObterTotalFatura(Guid idFatura);
};