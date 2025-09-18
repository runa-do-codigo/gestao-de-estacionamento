using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloFaturamento;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class FaturamentoModelsMappingProfile : Profile
{
    public FaturamentoModelsMappingProfile()
    {
        CreateMap<CadastrarFaturamentoRequest, CadastrarFaturamentoCommand>();
        CreateMap<CadastrarFaturamentoResult, CadastrarFaturamentoResponse>();

        CreateMap<(Guid, EditarFaturamentoRequest), EditarFaturamentoCommand>()
            .ConvertUsing(src => new EditarFaturamentoCommand(
                src.Item1,
                src.Item2.DataPagamento,
                src.Item2.ValorTotal,
                src.Item2.TicketId
            ));

        CreateMap<EditarFaturamentoResult, EditarFaturamentoResponse>()
            .ConvertUsing(src => new EditarFaturamentoResponse(
                src.DataPagamento,
                src.ValorTotal,
                src.TicketId
            ));

        CreateMap<Guid, ExcluirFaturamentoCommand>()
          .ConstructUsing(src => new ExcluirFaturamentoCommand(src));

        CreateMap<Guid, SelecionarFaturamentoPorIdQuery>()
            .ConvertUsing(src => new SelecionarFaturamentoPorIdQuery(src));

        CreateMap<SelecionarFaturamentoPorIdResult, SelecionarFaturamentoPorIdResponse>();

        CreateMap<SelecionarFaturamentosRequest, SelecionarFaturamentosQuery>();

        CreateMap<SelecionarFaturamentosResult, SelecionarFaturamentosResponse>()
            .ConvertUsing((src, dest, ctx) => new SelecionarFaturamentosResponse(
                src.Faturamentos.Count,
                src?.Faturamentos.Select(c => ctx.Mapper.Map<SelecionarFaturamentosDto>(c)).ToImmutableList() ?? ImmutableList<SelecionarFaturamentosDto>.Empty
            ));
    }
}