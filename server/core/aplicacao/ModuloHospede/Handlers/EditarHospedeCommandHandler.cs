using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Handlers;
public class EditarHospedeCommandHandler(
    IRepositorioHospede repositorioHospede,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<EditarHospedeCommand> validator,
    ILogger<EditarHospedeCommandHandler> logger
    ) : IRequestHandler<EditarHospedeCommand, Result<EditarHospedeResult>>
{
    public async Task<Result<EditarHospedeResult>> Handle(EditarHospedeCommand command, CancellationToken cancellationToken)
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
            var hospedeEditado = mapper.Map<Hospede>(command);

            hospedeEditado.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioHospede.EditarAsync(command.Id, hospedeEditado);

            await unitOfWork.CommitAsync();

            var result = mapper.Map<EditarHospedeResult>(hospedeEditado);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a edição de {@Registro}.",
                command
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}