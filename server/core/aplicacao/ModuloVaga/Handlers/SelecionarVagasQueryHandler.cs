using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;
partial class SelecionarVagasQueryHandler(
    IRepositorioVaga repositorioVaga,
    IMapper mapper,
    ILogger<SelecionarVagasQueryHandler> logger
    ) : IRequestHandler<SelecionarVagasQuery, Result<SelecionarVagasResult>>
{
    public async Task<Result<SelecionarVagasResult>> Handle(SelecionarVagasQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registros = query.Quantidade.HasValue ?
                await repositorioVaga.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioVaga.SelecionarRegistrosAsync();

            var result = mapper.Map<SelecionarVagasResult>(registros);

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