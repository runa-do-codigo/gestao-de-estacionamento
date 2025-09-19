using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloTicket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("tickets")]
public class TicketController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("cadastrar-ticket")]
    public async Task<ActionResult<CadastrarTicketResponse>> Cadastrar(CadastrarTicketRequest request)
    {
        var command = mapper.Map<CadastrarTicketCommand>(request);

        var result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                var errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var response = mapper.Map<CadastrarTicketResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid} editar-ticket")]
    public async Task<ActionResult<EditarTicketResponse>> Editar(Guid id, EditarTicketRequest request)
    {
        var command = mapper.Map<(Guid, EditarTicketRequest), EditarTicketCommand>((id, request));

        var result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadataKey("TipoErro")))
            {
                var errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var response = mapper.Map<EditarTicketResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid} excluir-ticket")]
    public async Task<ActionResult<ExcluirTicketResponse>> Excluir(Guid id)
    {
        var command = mapper.Map<ExcluirTicketCommand>(id);

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        return NoContent();
    }

    [HttpGet("selecionar-tickets")]
    public async Task<ActionResult<SelecionarTicketsResponse>> SelecionarRegistros(
        [FromQuery] SelecionarTicketsRequest? request,
        CancellationToken cancellationToken
    )
    {
        var query = mapper.Map<SelecionarTicketsQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarTicketsResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid} selecionar-ticket-por-id")]
    public async Task<ActionResult<SelecionarTicketPorIdResponse>> SelecionarRegistroPorId(Guid id)
    {
        var query = mapper.Map<SelecionarTicketPorIdQuery>(id);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(id);

        var response = mapper.Map<SelecionarTicketPorIdResponse>(result.Value);

        return Ok(response);
    }
}
