using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloHospede;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;


[ApiController]
[Authorize]
[Route("hospedes")]
public class HospedeController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("cadastrar-hospede")]
    public async Task<ActionResult<CadastrarHospedeResponse>> Cadastrar(CadastrarHospedeRequest request)
    {
        var command = mapper.Map<CadastrarHospedeCommand>(request);

        var result = await mediator.Send(command);

        if (result.IsFailed)
        {
            if (result.HasError(e => e.HasMetadata("TipoErro", m => m.Equals("RequisicaoInvalida"))))
            {
                var errosDeValidacao = result.Errors
                    .SelectMany(e => e.Reasons.OfType<IError>())
                    .Select(e => e.Message);

                return BadRequest(errosDeValidacao);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var response = mapper.Map<CadastrarHospedeResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid} editar-hospede")]
    public async Task<ActionResult<EditarHospedeResponse>> Editar(Guid id, EditarHospedeRequest request)
    {
        var command = mapper.Map<(Guid, EditarHospedeRequest), EditarHospedeCommand>((id, request));

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<EditarHospedeResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid} deletar-hospede")]
    public async Task<ActionResult<ExcluirHospedeResponse>> Excluir(Guid id)
    {
        var command = mapper.Map<ExcluirHospedeCommand>(id);

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        return NoContent();
    }

    [HttpGet("selecionar-hospedes")]
    public async Task<ActionResult<SelecionarHospedesResponse>> SelecionarRegistros(
        [FromQuery] SelecionarHospedesRequest? request,
        CancellationToken cancellationToken
    )
    {
        var query = mapper.Map<SelecionarHospedesQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarHospedesResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid} selecionar-hospede-por-id")]
    public async Task<ActionResult<SelecionarHospedePorIdResponse>> SelecionarRegistroPorId(Guid id)
    {
        var query = mapper.Map<SelecionarHospedePorIdQuery>(id);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(id);

        var response = mapper.Map<SelecionarHospedePorIdResponse>(result.Value);

        return Ok(response);
    }
}