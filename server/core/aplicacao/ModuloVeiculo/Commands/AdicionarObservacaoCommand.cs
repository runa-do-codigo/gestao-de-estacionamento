using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

public record AdicionarObservacaoCommand(
    Guid Id,
    string Observacao
) : IRequest<Result<AdicionarObservacaoResult>>;

public record AdicionarObservacaoResult(
    Guid Id,
    string Observacao
);