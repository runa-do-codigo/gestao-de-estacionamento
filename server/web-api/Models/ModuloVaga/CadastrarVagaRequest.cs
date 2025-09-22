namespace GestaoDeEstacionamento.WebApi.Models.ModuloVaga;

public record CadastrarVagaRequest(
    int Numero,
    bool EstaOcupada
    );

public record CadastrarVagaResponse(Guid Id);