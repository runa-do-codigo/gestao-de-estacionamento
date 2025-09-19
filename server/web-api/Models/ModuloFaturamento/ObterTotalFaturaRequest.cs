namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record ObterTotalFaturaRequest(Guid Id);

public record ObterTotalFaturaResponse(
    Guid Id,
    int ValorTotal
);