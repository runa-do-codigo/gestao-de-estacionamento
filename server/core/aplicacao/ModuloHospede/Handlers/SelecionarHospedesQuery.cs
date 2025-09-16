using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Handlers;
partial class SelecionarHospedesQueryHandler(
    IRepositorioHospede repositorioHospede,
    IMapper mapper,
    ILogger<SelecionarHospedesQueryHandler> logger
    ) : IRequestHandler<SelecionarHospedesQuery, Result<SelecionarHospedesResult>>
{
    public async Task<Result<SelecionarHospedesResult>> Handle(SelecionarHospedesQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registros = query.Quantidade.HasValue ?
                await repositorioHospede.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioHospede.SelecionarRegistrosAsync();

            var result = mapper.Map<SelecionarHospedesResult>(registros);

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