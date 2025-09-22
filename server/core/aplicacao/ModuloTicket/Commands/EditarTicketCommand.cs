using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

public record EditarTicketCommand(
    Guid Id,
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId,
    Guid VagaId
) : IRequest<Result<EditarTicketResult>>;

public record EditarTicketResult(
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId,
    Guid VagaId
);