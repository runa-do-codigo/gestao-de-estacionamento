namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record SelecionarVeiculoPorIdRequest(Guid Id);

public record SelecionarVeiculoPorIdResponse(
    Guid Id,
    string Placa,
    string Modelo,
    string Cor,
    string? Observacao,
    Guid HospedeId
    );