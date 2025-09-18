using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Handlers;

public class EditarFaturamentoCommandHandler(
    IRepositorioFaturamento repositorioFaturamento,
    IRepositorioTicket repositorioTicket,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<EditarFaturamentoCommand> validator,
    ILogger<EditarFaturamentoCommandHandler> logger
) : IRequestHandler<EditarFaturamentoCommand, Result<EditarFaturamentoResult>>
{
    public async Task<Result<EditarFaturamentoResult>> Handle(
        EditarFaturamentoCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);

            return Result.Fail(erroFormatado);
        }

        var registros = await repositorioFaturamento.SelecionarRegistrosAsync();

        if (registros.Any(i => !i.Id.Equals(command.Id) && i.Ticket.Equals(command.TicketId)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um compromisso registrado com este Ticket."));

        try
        {
            var faturamentoEditado = mapper.Map<Faturamento>(command);

            if (command.TicketId.HasValue)
                faturamentoEditado.Ticket = await repositorioTicket.SelecionarRegistroPorIdAsync(command.TicketId.Value);

            await repositorioFaturamento.EditarAsync(command.Id, faturamentoEditado);

            await unitOfWork.CommitAsync();

            // Invalida o cache
            var cacheKey = $"faturamentos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<EditarFaturamentoResult>(faturamentoEditado);

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