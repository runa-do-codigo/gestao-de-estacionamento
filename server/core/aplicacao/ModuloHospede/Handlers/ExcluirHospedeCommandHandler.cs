using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Handlers;
public class ExcluirHospedeCommandHandler(
    IRepositorioHospede repositorioHospede,
    IUnitOfWork unitOfWork,
    ILogger<ExcluirHospedeCommandHandler> logger
    ) : IRequestHandler<ExcluirHospedeCommand, Result<ExcluirHospedeResult>>
{
    public async Task<Result<ExcluirHospedeResult>> Handle(ExcluirHospedeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var hospedeSelecionado = await repositorioHospede.SelecionarRegistroPorIdAsync(command.Id);

            if (hospedeSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

            await repositorioHospede.ExcluirAsync(command.Id);

            await unitOfWork.CommitAsync();

            var result = new ExcluirHospedeResult();

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