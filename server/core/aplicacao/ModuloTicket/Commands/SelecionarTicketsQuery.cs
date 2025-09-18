using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

public record SelecionarTicketsQuery(int? Quantidade)
    : IRequest<Result<SelecionarTicketsResult>>;

public record SelecionarTicketsResult(ImmutableList<SelecionarTicketsDto> Tickets);

public record SelecionarTicketsDto(
    Guid Id,
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId/*
    Guid VagaId*/
);