﻿using AutoMapper;
using FluentResults;
using GestaoDeEstacionamento.Core.Aplicacao.Compartilhado;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloAutenticacao;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using GestaoDeEstacionamento.Core.Dominio.ModuloTicket;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Handlers;

public class SelecionarTicketsQueryHandler(
    IRepositorioTicket repositorioTicket,
    ITenantProvider tenantProvider,
    IMapper mapper,
    IDistributedCache cache,
    ILogger<SelecionarTicketsQueryHandler> logger
) : IRequestHandler<SelecionarTicketsQuery, Result<SelecionarTicketsResult>>
{
    public async Task<Result<SelecionarTicketsResult>> Handle(
        SelecionarTicketsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var cacheQuery = query.Quantidade.HasValue ? $"q={query.Quantidade.Value}" : "q=all";
            var cacheKey = $"tickets:u={tenantProvider.UsuarioId.GetValueOrDefault()}:{cacheQuery}";

            // 1) Tenta acessar o cache
            var jsonString = await cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                var resultadoEmCache = JsonSerializer.Deserialize<SelecionarTicketsResult>(jsonString);

                if (resultadoEmCache is not null)
                    return Result.Ok(resultadoEmCache);
            }

            // 2) Cache miss -> busca no repositório
            var registros = query.Quantidade.HasValue ?
                await repositorioTicket.SelecionarRegistrosAsync(query.Quantidade.Value) :
                await repositorioTicket.SelecionarRegistrosAsync();

            var result = mapper.Map<SelecionarTicketsResult>(registros);

            // 3) Salva os resultados novos no cache
            var jsonPayload = JsonSerializer.Serialize(result);

            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) };

            await cache.SetStringAsync(cacheKey, jsonPayload, cacheOptions, cancellationToken);

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