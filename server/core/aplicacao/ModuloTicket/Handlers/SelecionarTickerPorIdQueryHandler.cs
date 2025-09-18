using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class SelecionarTicketPorIdQueryHandler(
    IRepositorioTicket repositorioTicket,
    IMapper mapper,
    ILogger<SelecionarTicketPorIdQueryHandler> logger
) : IRequestHandler<SelecionarTicketPorIdQuery, Result<SelecionarTicketPorIdResult>>
{
    public async Task<Result<SelecionarTicketPorIdResult>> Handle(
        SelecionarTicketPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioTicket.SelecionarRegistroPorIdAsync(query.Id);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            var result = mapper.Map<SelecionarTicketPorIdResult>(registro);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de {@Registro}.",
                query
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
