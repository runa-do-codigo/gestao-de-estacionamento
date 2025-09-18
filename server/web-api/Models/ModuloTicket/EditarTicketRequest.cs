namespace GestaoDeEstacionamento.WebApi.Models.ModuloTicket;

public record EditarTicketRequest(
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId/*,
    Guid VagaId*/
);

public record EditarTicketResponse(
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId/*,
    Guid VagaId*/
);