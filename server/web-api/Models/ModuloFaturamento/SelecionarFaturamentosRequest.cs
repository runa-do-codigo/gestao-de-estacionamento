using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record SelecionarFaturamentosRequest(int? Quantidade);

public record SelecionarFaturamentosResponse(
    int Quantidade,
    ImmutableList<SelecionarFaturamentosDto> Faturamentos
);