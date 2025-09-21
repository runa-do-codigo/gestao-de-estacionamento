using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;
public class ExcluirVagaCommandHandler(
    IRepositorioVaga repositorioVaga,
    IUnitOfWork unitOfWork,
    ILogger<ExcluirVagaCommandHandler> logger
    ) : IRequestHandler<ExcluirVagaCommand, Result<ExcluirVagaResult>>
{
    public async Task<Result<ExcluirVagaResult>> Handle(ExcluirVagaCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var vagaSelecionado = await repositorioVaga.SelecionarRegistroPorIdAsync(command.Id);

            if (vagaSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

            await repositorioVaga.ExcluirAsync(command.Id);

            await unitOfWork.CommitAsync();

            var result = new ExcluirVagaResult();

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