using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
public record CadastrarHospedeCommand(string Nome,string CPF) : IRequest<Result<CadastrarHospedeResult>>;

public record CadastrarHospedeResult(Guid Id);