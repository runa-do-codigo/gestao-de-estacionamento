using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
public record EditarHospedeCommand(Guid Id,string Nome,string CPF) : IRequest<Result<EditarHospedeResult>>;

public record EditarHospedeResult(string Nome,string CPF);