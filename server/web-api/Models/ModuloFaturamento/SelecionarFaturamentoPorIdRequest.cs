namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record SelecionarFaturamentoPorIdRequest(Guid Id);

public record SelecionarFaturamentoPorIdResponse(
    Guid Id,
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
);