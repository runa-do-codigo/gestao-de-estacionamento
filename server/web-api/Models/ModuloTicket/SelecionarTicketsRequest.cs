using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.Models.ModuloTicket;

public record SelecionarTicketsRequest(int? Quantidade);

public record SelecionarTicketsResponse(
    int Quantidade,
    ImmutableList<SelecionarTicketsDto> Tickets
);