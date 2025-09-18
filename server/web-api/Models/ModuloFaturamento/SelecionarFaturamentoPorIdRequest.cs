namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record SelecionarFaturamentoPorIdRequest(Guid Id);

public record SelecionarFaturamentoPorIdResponse(
    Guid Id,
    DateTime HoraPagamento,
    int ValorTotal,
    Guid? TicketId = null
);