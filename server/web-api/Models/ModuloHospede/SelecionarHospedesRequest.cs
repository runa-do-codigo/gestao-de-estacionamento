using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloHospede;

public record SelecionarHospedesRequest(int? Quantidade);

public record SelecionarHospedesResponse(
    int Quantidade,
    ImmutableList<SelecionarHospedesDto> Hospedes
    );