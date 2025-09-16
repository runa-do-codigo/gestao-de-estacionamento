using AutoMapper;
using eAgenda.Core.Aplicacao.Compartilhado;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Handlers;
public class CadastrarHospedeCommandHandler(
    IRepositorioHospede repositorioHospede,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CadastrarHospedeCommand> validator,
    ILogger<CadastrarHospedeCommandHandler> logger
    ) : IRequestHandler<CadastrarHospedeCommand, Result<CadastrarHospedeResult>>
{
    public async Task<Result<CadastrarHospedeResult>> Handle(CadastrarHospedeCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);

            return Result.Fail(erroFormatado);
        }

        try
        {
            var hospede = mapper.Map<Hospede>(command);

            await repositorioHospede.CadastrarAsync(hospede);

            await unitOfWork.CommitAsync();

            var result = mapper.Map<CadastrarHospedeResult>(hospede);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@Registro}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}