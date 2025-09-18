using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
public record CadastrarVeiculoCommand(
    string Placa,
    string Modelo,
    string Cor
    ) : IRequest<Result<CadastrarVeiculoResult>>;

public record CadastrarVeiculoResult(Guid Id);