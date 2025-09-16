namespace GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;

public record CadastrarFaturamentoRequest(
    DateTime DataInicio, 
    DateTime DataFim,
    TimeOnly HoraPagamento,
    int ValorTotal,
    Guid? TicketId = null
);

public record CadastrarFaturamentoResponse(Guid Id);