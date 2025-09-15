using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModulFaturamento.Commands;

public record SelecionarFaturamentoPorIdQuery(Guid Id) : IRequest<Result<SelecionarFaturamentoPorIdResult>>;

public record SelecionarFaturamentoPorIdResult(
    Guid Id,
    DateTime DataInicio,
    DateTime? DataFim,
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid? TicketId
);