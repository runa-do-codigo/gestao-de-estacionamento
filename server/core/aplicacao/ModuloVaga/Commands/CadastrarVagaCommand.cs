using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
public record CadastrarVagaCommand(
    int Numero,
    bool EstaOcupada
    ) : IRequest<Result<CadastrarVagaResult>>;

public record CadastrarVagaResult(Guid Id);