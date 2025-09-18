namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record EditarFaturamentoRequest(
    DateTime DataPagamento,
    int ValorTotal,
    Guid? TicketId = null
);

public record EditarFaturamentoResponse(
    DateTime? HoraPagamento,
    int? ValorTotal,
    Guid? TicketId = null
);