using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;

public record SelecionarFaturamentosQuery(int? Quantidade)
    : IRequest<Result<SelecionarFaturamentosResult>>;

public record SelecionarFaturamentosResult(ImmutableList<SelecionarFaturamentosDto> Faturamentos);

public record SelecionarFaturamentosDto(
    Guid Id,
    DateTime DataInicio,
    DateTime? DataFim,
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
);