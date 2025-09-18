using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;

public record EditarFaturamentoCommand(
    Guid Id,
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
) : IRequest<Result<EditarFaturamentoResult>>;

public record EditarFaturamentoResult(
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
);