using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eAgenda.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("faturamentos")]
public class FaturamentoController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CadastrarFaturamentoResponse>> Cadastrar(CadastrarFaturamentoRequest request)
    {
        var command = mapper.Map<CadastrarFaturamentoCommand>(request);

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

        var response = mapper.Map<CadastrarFaturamentoResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EditarFaturamentoResponse>> Editar(Guid id, EditarFaturamentoRequest request)
    {
        var command = mapper.Map<(Guid, EditarFaturamentoRequest), EditarFaturamentoCommand>((id, request));

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

        var response = mapper.Map<EditarFaturamentoResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ExcluirFaturamentoResponse>> Excluir(Guid id)
    {
        var command = mapper.Map<ExcluirFaturamentoCommand>(id);

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<SelecionarFaturamentosResponse>> SelecionarRegistros(
        [FromQuery] SelecionarFaturamentosRequest? request,
        CancellationToken cancellationToken
    )
    {
        var query = mapper.Map<SelecionarFaturamentosQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarFaturamentosResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelecionarFaturamentoPorIdResponse>> SelecionarRegistroPorId(Guid id)
    {
        var query = mapper.Map<SelecionarFaturamentoPorIdQuery>(id);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(id);

        var response = mapper.Map<SelecionarFaturamentoPorIdResponse>(result.Value);

        return Ok(response);
    }
}
