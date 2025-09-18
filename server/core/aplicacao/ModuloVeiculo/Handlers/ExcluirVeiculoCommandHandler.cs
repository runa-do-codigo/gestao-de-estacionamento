using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Handlers;
public class ExcluirVeiculoCommandHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<ExcluirVeiculoCommandHandler> logger
    ) : IRequestHandler<ExcluirVeiculoCommand, Result<ExcluirVeiculoResult>>
{
    public async Task<Result<ExcluirVeiculoResult>> Handle(ExcluirVeiculoCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var veiculoSelecionado = await repositorioVeiculo.SelecionarRegistroPorIdAsync(command.Id);

            if (veiculoSelecionado is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(command.Id));

            await repositorioVeiculo.ExcluirAsync(command.Id);

            await unitOfWork.CommitAsync();

            var result = new ExcluirVeiculoResult();

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