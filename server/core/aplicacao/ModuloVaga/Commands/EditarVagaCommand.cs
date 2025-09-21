using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
public record EditarVagaCommand(
    Guid Id,
    int Numero,
    bool EstaOcupada
    ) : IRequest<Result<EditarVagaResult>>;

public record EditarVagaResult(
    int Numero,
    bool Estaocupada
    );