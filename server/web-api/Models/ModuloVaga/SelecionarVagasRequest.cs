using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloVaga;

public record SelecionarVagasRequest(int? Quantidade);

public record SelecionarVagasResponse(
    int Quantidade,
    ImmutableList<SelecionarVagasDto> Vagas
    );