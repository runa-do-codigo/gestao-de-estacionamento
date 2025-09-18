namespace GestaoDeEstacionamento.WebApi.Models.ModuloTicket;

public record CadastrarTicketRequest(
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId/*,
    Guid VagaId*/
);

public record CadastrarTicketResponse(Guid Id);