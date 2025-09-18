using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

public record ExcluirTicketCommand(Guid Id) : IRequest<Result<ExcluirTicketResult>>;

public record ExcluirTicketResult();