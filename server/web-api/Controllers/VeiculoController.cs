﻿using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEstacionamento.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("veiculos")]
public class VeiculoController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost("cadastrar-veiculo")]
    public async Task<ActionResult<CadastrarVeiculoResponse>> Cadastrar(CadastrarVeiculoRequest request)
    {
        var command = mapper.Map<CadastrarVeiculoCommand>(request);

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

        var response = mapper.Map<CadastrarVeiculoResponse>(result.Value);

        return Created(string.Empty, response);
    }

    [HttpPut("{id:guid} editar-veiculo")]
    public async Task<ActionResult<EditarVeiculoResponse>> Editar(Guid id, EditarVeiculoRequest request)
    {
        var command = mapper.Map<(Guid, EditarVeiculoRequest), EditarVeiculoCommand>((id, request));

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<EditarVeiculoResponse>(result.Value);

        return Ok(response);
    }

    [HttpDelete("{id:guid} deletar-veiculo")]
    public async Task<ActionResult<ExcluirVeiculoResponse>> Excluir(Guid id)
    {
        var command = mapper.Map<ExcluirVeiculoCommand>(id);

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest();

        return NoContent();
    }

    [HttpGet("selecionar-veiculos")]
    public async Task<ActionResult<SelecionarVeiculosResponse>> SelecionarRegistros(
        [FromQuery] SelecionarVeiculosRequest? request,
        CancellationToken cancellationToken
    )
    {
        var query = mapper.Map<SelecionarVeiculosQuery>(request);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailed)
            return BadRequest();

        var response = mapper.Map<SelecionarVeiculosResponse>(result.Value);

        return Ok(response);
    }

    [HttpGet("{id:guid} selecionar-veiculo-por-id")]
    public async Task<ActionResult<SelecionarVeiculoPorIdResponse>> SelecionarRegistroPorId(Guid id)
    {
        var query = mapper.Map<SelecionarVeiculoPorIdQuery>(id);

        var result = await mediator.Send(query);

        if (result.IsFailed)
            return NotFound(id);

        var response = mapper.Map<SelecionarVeiculoPorIdResponse>(result.Value);

        return Ok(response);
    }
    [HttpPatch("{id:guid} adicionar-observacao")]
    public async Task<ActionResult<AdicionarObservacaoResponse>> AdicionarObservacao(
    Guid id,
    AdicionarObservacaoRequest request)
    {
        var command = mapper.Map<(Guid, string), AdicionarObservacaoCommand>((id, request.Observacao));

        var result = await mediator.Send(command);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(e => e.Message));

        var response = mapper.Map<AdicionarObservacaoResponse>(result.Value);

        return Ok(response);
    }
}