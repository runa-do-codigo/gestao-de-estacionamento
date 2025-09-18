using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloTicket;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class TicketModelsMappingProfile : Profile
{
    public TicketModelsMappingProfile()
    {
        CreateMap<CadastrarTicketRequest, CadastrarTicketCommand>();
        CreateMap<CadastrarTicketResult, CadastrarTicketResponse>();

        CreateMap<(Guid, EditarTicketRequest), EditarTicketCommand>()
            .ConvertUsing(src => new EditarTicketCommand(
                src.Item1,
                src.Item2.DataEntrada,
                src.Item2.DataSaida,
                src.Item2.VeiculoId/*,
                src.Item2.VagaId*/
            ));

        CreateMap<EditarTicketResult, EditarTicketResponse>()
            .ConvertUsing(src => new EditarTicketResponse(
                src.DataEntrada,
                src.DataSaida,
                src.VeiculoId/*,
                src.Item2.VagaId*/
            ));

        CreateMap<Guid, ExcluirTicketCommand>()
          .ConstructUsing(src => new ExcluirTicketCommand(src));

        CreateMap<Guid, SelecionarTicketPorIdQuery>()
            .ConvertUsing(src => new SelecionarTicketPorIdQuery(src));

        CreateMap<SelecionarTicketPorIdResult, SelecionarTicketPorIdResponse>();

        CreateMap<SelecionarTicketsRequest, SelecionarTicketsQuery>();

        CreateMap<SelecionarTicketsResult, SelecionarTicketsResponse>()
            .ConvertUsing((src, dest, ctx) => new SelecionarTicketsResponse(
                src.Tickets.Count,
                src?.Tickets.Select(c => ctx.Mapper.Map<SelecionarTicketsDto>(c)).ToImmutableList() ?? ImmutableList<SelecionarTicketsDto>.Empty
            ));
    }
}