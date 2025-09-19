using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Queries;

public record ObterTotalFaturaQuery(Guid IdFatura)
    : IRequest<Result<ObterTotalFaturaResult>>;

public record ObterTotalFaturaResult(
    Guid Id,
    int? ValorTotal,
    Guid TicketId
);