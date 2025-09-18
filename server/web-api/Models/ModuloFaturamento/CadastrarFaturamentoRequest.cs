namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record CadastrarFaturamentoRequest(
    DateTime? DataPagamento,
    int? ValorTotal,
    Guid TicketId
);

public record CadastrarFaturamentoResponse(Guid Id);