using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;

public record CadastrarFaturamentoCommand(
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
) : IRequest<Result<CadastrarFaturamentoResult>>;

public record CadastrarFaturamentoResult(Guid Id);