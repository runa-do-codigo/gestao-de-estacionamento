using AutoMapper;
using FluentResults;
using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.Compartilhado;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class CadastrarTicketCommandHandler(
    IRepositorioTicket repositorioTicket,
    IRepositorioTicket repositorioVeiculo,
    IRepositorioVaga repositorioVaga,
    ITenantProvider tenantProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDistributedCache cache,
    IValidator<CadastrarTicketCommand> validator,
    ILogger<CadastrarTicketCommandHandler> logger
) : IRequestHandler<CadastrarTicketCommand, Result<CadastrarTicketResult>>
{
    public async Task<Result<CadastrarTicketResult>> Handle(
        CadastrarTicketCommand command, CancellationToken cancellationToken)
    {
        var resultadoValidacao = await validator.ValidateAsync(command, cancellationToken);

        if (!resultadoValidacao.IsValid)
        {
            var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);

            var erroFormatado = ResultadosErro.RequisicaoInvalidaErro(erros);

            return Result.Fail(erroFormatado);
        }

        var registros = await repositorioTicket.SelecionarRegistrosAsync();

        if (registros.Any(i => i.Veiculo.Equals(command.VeiculoId)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um ticket registrado com este Veiculo."));

        if (registros.Any(i => i.Vaga.Equals(command.VagaId)))
            return Result.Fail(ResultadosErro.RegistroDuplicadoErro("Já existe um ticket registrado com este Vaga."));
        try
        {
            var ticket = mapper.Map<Ticket>(command);

            //if (command.VeiculoId.HasValue)
                //ticket.Veiculo = await repositorioTicket.SelecionarRegistroPorIdAsync(command.VeiculoId.Value);

            //if (command.VagaId.HasValue)
                //ticket.Vaga = await repositorioTicket.SelecionarRegistroPorIdAsync(command.VagaId.Value);

            ticket.UsuarioId = tenantProvider.UsuarioId.GetValueOrDefault();

            await repositorioTicket.CadastrarAsync(ticket);

            await unitOfWork.CommitAsync();

            // Invalida o cache
            var cacheKey = $"compromissos:u={tenantProvider.UsuarioId.GetValueOrDefault()}:q=all";

            await cache.RemoveAsync(cacheKey, cancellationToken);

            var result = mapper.Map<CadastrarTicketResult>(ticket);

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