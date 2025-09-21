﻿using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVaga;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;


[ApiController]
[Authorize]
[Route("vagas")]
public class VagaController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("cadastrar-vaga")]
    public async Task<ActionResult<CadastrarVagaResponse>> Cadastrar(CadastrarVagaRequest request)
    {
        var command = mapper.Map<CadastrarVagaCommand>(request);

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

        var response = mapper.Map<CadastrarVagaResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid} editar-vaga")]
    public async Task<ActionResult<EditarVagaResponse>> Editar(Guid id, EditarVagaRequest request)
    {
        var command = mapper.Map<(Guid, EditarVagaRequest), EditarVagaCommand>((id, request));

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<EditarVagaResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid} deletar-vaga")]
    public async Task<ActionResult<ExcluirVagaResponse>> Excluir(Guid id)
    {
        var command = mapper.Map<ExcluirVagaCommand>(id);

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        return NoContent();
    }

    [HttpGet("selecionar-vagas")]
    public async Task<ActionResult<SelecionarVagasResponse>> SelecionarRegistros(
        [FromQuery] SelecionarVagasRequest? request,
        CancellationToken cancellationToken
    )
    {
        var query = mapper.Map<SelecionarVagasQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarVagasResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid} selecionar-vaga-por-id")]
    public async Task<ActionResult<SelecionarVagaPorIdResponse>> SelecionarRegistroPorId(Guid id)
    {
        var query = mapper.Map<SelecionarVagaPorIdQuery>(id);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(id);

        var response = mapper.Map<SelecionarVagaPorIdResponse>(result.Value);

        return Ok(response);
    }
}