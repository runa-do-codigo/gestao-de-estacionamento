namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record EditarFaturamentoRequest(
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
);

public record EditarFaturamentoResponse(
    DateTime? HoraPagamento,
    int? ValorTotal,
    Guid TicketId
);