using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloHospede;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Handlers;
public class SelecionarHospedePorIdQueryHandler(
    IMapper mapper,
    IRepositorioHospede repositorioHospede,
    ILogger<SelecionarHospedePorIdQueryHandler> logger
    ) : IRequestHandler<SelecionarHospedePorIdQuery, Result<SelecionarHospedePorIdResult>>
{
    public async Task<Result<SelecionarHospedePorIdResult>> Handle(SelecionarHospedePorIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var registro = await repositorioHospede.SelecionarRegistroPorIdAsync(query.Id);

            if (registro is null)
                return Result.Fail(ResultadosErro.RegistroNaoEncontradoErro(query.Id));

            var result = mapper.Map<SelecionarHospedePorIdResult>(registro);

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