using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
public record ExcluirHospedeCommand(Guid Id) : IRequest<Result<ExcluirHospedeResult>>;

public record ExcluirHospedeResult();