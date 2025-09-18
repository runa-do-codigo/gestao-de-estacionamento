using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturament.Handlers;

public class ExcluirFaturamentoCommandHandler(
    IRepositorioFaturamento repositorioFaturamento,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IDistributedCache cache,
    ILogger<ExcluirFaturamentoCommandHandler> logger
) : IRequestHandler<ExcluirFaturamentoCommand, Result<ExcluirFaturamentoResult>>
{
    public async Task<Result<ExcluirFaturamentoResult>> Handle(
        ExcluirFaturamentoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await repositorioFaturamento.ExcluirAsync(command.Id);

            await unitOfWork.CommitAsync();

            var cacheKey = $"faturamentos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = new ExcluirFaturamentoResult();

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão de {@Registro}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
