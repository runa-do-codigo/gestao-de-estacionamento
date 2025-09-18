using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVeiculo;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Handlers;
public class SelecionarVeiculosQueryHandler(
    IRepositorioVeiculo repositorioVeiculo,
    IMapper mapper,
    ILogger<SelecionarVeiculosQueryHandler> logger
    ) : IRequestHandler<SelecionarVeiculosQuery, Result<SelecionarVeiculosResult>>
{
    public async Task<Result<SelecionarVeiculosResult>> Handle(SelecionarVeiculosQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registros = query.Quantidade.HasValue ?
                await repositorioVeiculo.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioVeiculo.SelecionarRegistrosAsync();

            var result = mapper.Map<SelecionarVeiculosResult>(registros);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro durante a seleção de {@Registros}.",
                query
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}