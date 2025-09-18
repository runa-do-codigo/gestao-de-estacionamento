using FluentResults;
using MediatR;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

public record CadastrarTicketCommand(
    DateTime DataEntrada,
    DateTime? DataSaida,
    Guid VeiculoId/*,
    Guid VagaId*/
) : IRequest<Result<CadastrarTicketResult>>;

public record CadastrarTicketResult(Guid Id);