namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record EditarVeiculoRequest(
    string Placa,
    string Modelo,
    string Cor,
    string? Observacao,
    Guid HospedeId
    );

public record EditarVeiculoResponse(
    string Placa,
    string Modelo,
    string Cor,
    string? Observacao,
    Guid HospedeId
    );