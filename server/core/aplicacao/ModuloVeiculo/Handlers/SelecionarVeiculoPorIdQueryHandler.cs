using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Handlers;
public class SelecionarVeiculoPorIdQueryHandler(
    IMapper mapper,
    IRepositorioVeiculo repositorioVeiculo,
    ILogger<SelecionarVeiculoPorIdQueryHandler> logger
    ) : IRequestHandler<SelecionarVeiculoPorIdQuery, Result<SelecionarVeiculoPorIdResult>>
{
    public async Task<Result<SelecionarVeiculoPorIdResult>> Handle(SelecionarVeiculoPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioVeiculo.SelecionarRegistroPorIdAsync(query.Id);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            var result = mapper.Map<SelecionarVeiculoPorIdResult>(registro);

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