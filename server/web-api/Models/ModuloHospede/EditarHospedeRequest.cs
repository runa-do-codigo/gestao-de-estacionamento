namespace GestaoDeEstacionamento.WebApi.Models.ModuloHospede;

public record EditarHospedeRequest(
    string Nome,
    string CPF
    );

public record EditarHospedeResponse(
    string Nome,
    string CPF
    );