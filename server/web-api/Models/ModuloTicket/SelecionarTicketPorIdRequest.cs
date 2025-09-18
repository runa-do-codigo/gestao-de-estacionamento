namespace GestaoDeEstacionamento.WebApi.Models.ModuloTicket;

public record SelecionarTicketPorIdRequest(Guid Id);

public record SelecionarTicketPorIdResponse(
    Guid Id,
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId/*,
    Guid VagaId*/
);