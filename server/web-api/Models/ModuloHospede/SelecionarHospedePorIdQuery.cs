namespace GestaoDeEstacionamento.WebApi.Models.ModuloHospede;

public record SelecionarHospedePorIdRequest(Guid Id);

public record SelecionarHospedePorIdResponse(
    Guid Id,
    string Nome,
    string CPF
    );