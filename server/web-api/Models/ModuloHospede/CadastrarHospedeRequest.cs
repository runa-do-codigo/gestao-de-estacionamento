namespace GestaoDeEstacionamento.WebApi.Models.ModuloHospede;

public record CadastrarHospedeRequest(
    string Nome,
    string CPF
    );

public record CadastrarHospedeResponse(Guid Id);