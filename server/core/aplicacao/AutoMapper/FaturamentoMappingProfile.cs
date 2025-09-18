using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;
using GestaoDeEstacionamento.Core.Dominio.ModuloFaturamento;
using System.Collections.Immutable;

namespace eAgenda.Core.Aplicacao.AutoMapper;

public class FaturamentoMappingProfile : Profile
{
    public FaturamentoMappingProfile()
    {
        CreateMap<CadastrarFaturamentoCommand, Faturamento>();
        CreateMap<Faturamento, CadastrarFaturamentoResult>();

        CreateMap<EditarFaturamentoCommand, Faturamento>();
        CreateMap<Faturamento, EditarFaturamentoResult>();

        CreateMap<Faturamento, SelecionarFaturamentoPorIdResult>()
            .ConvertUsing(src => new SelecionarFaturamentoPorIdResult(
                src.Id,
                src.DataPagamento,
                src.ValorTotal,
                src.Ticket.Id 
            ));

        CreateMap<Faturamento, SelecionarFaturamentosDto>()
           .ConvertUsing(src => new SelecionarFaturamentosDto(
                src.Id,
                src.DataPagamento,
                src.ValorTotal,
                src.Ticket.Id
            ));

        CreateMap<IEnumerable<Faturamento>, SelecionarFaturamentosResult>()
         .ConvertUsing((src, dest, ctx) =>
             new SelecionarFaturamentosResult(
                 src?.Select(c => ctx.Mapper.Map<SelecionarFaturamentosDto>(c)).ToImmutableList() ?? ImmutableList<SelecionarFaturamentosDto>.Empty
             )
         );
    }
}