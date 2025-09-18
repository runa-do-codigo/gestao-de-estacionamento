using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class EditarTicketCommandHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioTicket repositorioVeiculo,
    /*IRepositorioVaga repositorioVaga,*/
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<EditarTicketCommand> validator,
    ILogger<EditarTicketCommandHandler> logger
) : IRequestHandler<EditarTicketCommand, Result<EditarTicketResult>>
{
    public async Task<Result<EditarTicketResult>> Handle(
        EditarTicketCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);

            return Result.Fail(erroFormatado);
        }

        var registros = await repositorioTicket.SelecionarRegistrosAsync();

        if (registros.Any(i => !i.Id.Equals(command.Id) && i.Veiculo.Equals(command.VeiculoId)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um Ticket registrado com este Veiculo."));

        //if (registros.Any(i => !i.Id.Equals(command.Id) && i.Vaga.Equals(command.VagaId)))
            //return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um Ticket registrado com esta Vaga."));

        try
        {
            var ticketEditado = mapper.Map<Ticket>(command);

            //if (command.VeiculoId.HasValue)
                //ticketEditado.Veiculo = await repositorioTicket.SelecionarRegistroPorIdAsync(command.VeiculoId.Value);

            //if (command.VagaId.HasValue)
                //ticketEditado.Vaga = await repositorioTicket.SelecionarRegistroPorIdAsync(command.VagaId.Value);

            await repositorioTicket.EditarAsync(command.Id, ticketEditado);

            await unitOfWork.CommitAsync();

            // Invalida o cache
            var cacheKey = $"tickets:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<EditarTicketResult>(ticketEditado);

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