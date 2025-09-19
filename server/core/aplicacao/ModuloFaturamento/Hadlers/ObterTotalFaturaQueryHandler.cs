using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Queries;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Handlers;

public class ObterTotalFaturaQueryHandler(
    IRepositorioFaturamento repositorioFaturamento,
    IMapper mapper,
    ILogger<ObterTotalFaturaQueryHandler> logger
) : IRequestHandler<ObterTotalFaturaQuery, Result<ObterTotalFaturaResult>>
{
    public async Task<Result<ObterTotalFaturaResult>> Handle(
        ObterTotalFaturaQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioFaturamento.ObterTotalFatura(query.IdFatura);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.IdFatura));

            var result = mapper.Map<ObterTotalFaturaResult>(registro);

            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ocorreu um erro ao obter o total da fatura {@Registro}.",
                query
            );

            return Result.Fail(ResultadosErro.ExcecaoInternaErro(ex));
        }
    }
}
