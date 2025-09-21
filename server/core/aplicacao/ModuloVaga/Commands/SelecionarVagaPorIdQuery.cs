using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
public record SelecionarVagaPorIdQuery(Guid Id) : IRequest<Result<SelecionarVagaPorIdResult>>;

public record SelecionarVagaPorIdResult(
    Guid Id,
    int Numero,
    bool EstaOcupada
    );