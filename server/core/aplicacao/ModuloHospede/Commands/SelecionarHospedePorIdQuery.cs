using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
public record SelecionarHospedePorIdQuery(Guid Id) : IRequest<Result<SelecionarHospedePorIdResult>>;

public record SelecionarHospedePorIdResult(Guid Id,string Nome,string CPF);