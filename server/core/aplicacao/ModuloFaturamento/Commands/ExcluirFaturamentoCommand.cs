using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;

public record ExcluirFaturamentoCommand(Guid Id) : IRequest<Result<ExcluirFaturamentoResult>>;

public record ExcluirFaturamentoResult();