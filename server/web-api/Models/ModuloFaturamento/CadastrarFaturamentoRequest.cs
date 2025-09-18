namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record CadastrarFaturamentoRequest(
    TimeOnly HoraPagamento,
    int ValorTotal,
    Guid? TicketId = null
);

public record CadastrarFaturamentoResponse(Guid Id);