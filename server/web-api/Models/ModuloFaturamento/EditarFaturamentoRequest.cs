namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record EditarFaturamentoRequest(
    DateTime DataInicio,
    DateTime DataFim,
    DateTime DataPagamento,
    int ValorTotal,
    Guid? TicketId = null
);

public record EditarFaturamentoResponse(
    DateTime DataInicio,
    DateTime? DataFim,
    DateTime? HoraPagamento,
    int? ValorTotal,
    Guid? TicketId = null
);