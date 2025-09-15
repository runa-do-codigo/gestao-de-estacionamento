using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModulFaturamento.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Handlers;

public class SelecionarFaturamentoPorIdQueryHandler(
    IRepositorioFaturamento repositorioFaturamento,
    IMapper mapper,
    ILogger<SelecionarFaturamentoPorIdQueryHandler> logger
) : IRequestHandler<SelecionarFaturamentoPorIdQuery, Result<SelecionarFaturamentoPorIdResult>>
{
    public async Task<Result<SelecionarFaturamentoPorIdResult>> Handle(
        SelecionarFaturamentoPorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioFaturamento.SelecionarRegistroPorIdAsync(query.Id);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            var result = mapper.Map<SelecionarFaturamentoPorIdResult>(registro);

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
