using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Handlers;

public class CadastrarFaturamentoCommandHandler(
    IRepositorioFaturamento repositorioFaturamento,
    IRepositorioTicket repositorioTicket,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<CadastrarFaturamentoCommand> validator,
    ILogger<CadastrarFaturamentoCommandHandler> logger
) : IRequestHandler<CadastrarFaturamentoCommand, Result<CadastrarFaturamentoResult>>
{
    public async Task<Result<CadastrarFaturamentoResult>> Handle(
        CadastrarFaturamentoCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);

            return Result.Fail(erroFormatado);
        }

        var registros = await repositorioFaturamento.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Ticket.Equals(command.TicketId)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um faturamento registrado com este Ticket."));

        try
        {
            var faturamento = mapper.Map<Faturamento>(command);

            if (command.TicketId.HasValue)
                faturamento.Ticket = await repositorioTicket.SelecionarRegistroPorIdAsync(command.TicketId.Value);

            faturamento.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioFaturamento.CadastrarAsync(faturamento);

            await unitOfWork.CommitAsync();

            // Invalida o cache
            var cacheKey = $"compromissos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<CadastrarFaturamentoResult>(faturamento);

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