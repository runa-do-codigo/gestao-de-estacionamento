using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

public record SelecionarTicketPorIdQuery(Guid Id) : IRequest<Result<SelecionarTicketPorIdResult>>;

public record SelecionarTicketPorIdResult(
    Guid Id,
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId,
    Guid VagaId
);