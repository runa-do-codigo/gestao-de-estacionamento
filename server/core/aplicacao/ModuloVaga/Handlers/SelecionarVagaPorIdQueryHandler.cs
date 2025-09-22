using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloVaga;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Handlers;
public class SelecionarVagaPorIdQueryHandler(
    IMapper mapper,
    IRepositorioVaga repositorioVaga,
    ILogger<SelecionarVagaPorIdQueryHandler> logger
    ) : IRequestHandler<SelecionarVagaPorIdQuery, Result<SelecionarVagaPorIdResult>>
{
    public async Task<Result<SelecionarVagaPorIdResult>> Handle(SelecionarVagaPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioVaga.SelecionarRegistroPorIdAsync(query.Id);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            var result = mapper.Map<SelecionarVagaPorIdResult>(registro);

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