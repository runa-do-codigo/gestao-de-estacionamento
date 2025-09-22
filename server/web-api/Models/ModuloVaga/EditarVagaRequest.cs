namespace GestaoDeEstacionamento.WebApi.Models.ModuloVaga;

public record EditarVagaRequest(
    int Numero,
    bool EstaOcupada
    );

public record EditarVagaResponse(
    int Numero,
    bool EstaOcupada
    );