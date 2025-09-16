using FluentResults;
using MediatR;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
public record SelecionarHospedesQuery(int? Quantidade) : IRequest<Result<SelecionarHospedesResult>>;

public record SelecionarHospedesResult(ImmutableList<SelecionarHospedesDto> Hospedes);

public record SelecionarHospedesDto(Guid Id,string Nome,string CPF);