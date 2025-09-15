using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;

public record EditarFaturamentoCommand(
    Guid Id,
    DateTime DataInicio,
    DateTime? DataFim,
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid? TicketId = null
) : IRequest<Result<EditarFaturamentoResult>>;

public record EditarFaturamentoResult(
    DateTime DataInicio,
    DateTime? DataFim,
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid? TicketId
);