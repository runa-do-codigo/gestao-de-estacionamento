using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
public record ExcluirVagaCommand(Guid Id) : IRequest<Result<ExcluirVagaResult>>;

public record ExcluirVagaResult();