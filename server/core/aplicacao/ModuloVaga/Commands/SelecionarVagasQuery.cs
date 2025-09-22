using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
public record SelecionarVagasQuery(int? Quantidade) : IRequest<Result<SelecionarVagasResult>>;

public record SelecionarVagasResult(ImmutableList<SelecionarVagasDto> Vagas);

public record SelecionarVagasDto(
    Guid Id,
    int Numero,
    bool EstaOcupada
    );