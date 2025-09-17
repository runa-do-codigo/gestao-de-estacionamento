using AutoMapper;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;
using GestaoDeEstacionamento.WebApi.Models.ModuloHospede;
using System.Collections.Immutable;

namespace GestaoDeEstacionamento.WebApi.AutoMapper;

public class HospedeModelsMappingProfile : Profile
{
    public HospedeModelsMappingProfile()
    {
        CreateMap<CadastrarHospedeRequest, CadastrarHospedeCommand>();
        CreateMap<CadastrarHospedeResult, CadastrarHospedeResponse>();

        CreateMap<(Guid, EditarHospedeRequest), EditarHospedeCommand>()
            .ConvertUsing(src => new EditarHospedeCommand(
                src.Item1,
                src.Item2.Nome,
                src.Item2.CPF
            ));

        CreateMap<EditarHospedeResult, EditarHospedeResponse>();

        CreateMap<Guid, ExcluirHospedeCommand>()
            .ConstructUsing(src => new ExcluirHospedeCommand(src));

        CreateMap<SelecionarHospedePorIdResult, SelecionarHospedePorIdResponse>()
            .ConvertUsing(src => new SelecionarHospedePorIdResponse(
                src.Id,
                src.Nome,
                src.CPF
            ));

        CreateMap<SelecionarHospedesRequest, SelecionarHospedesQuery>();

        CreateMap<SelecionarHospedesResult, SelecionarHospedesResponse>()
       .ConvertUsing((src, dest, ctx) => new SelecionarHospedesResponse(
           src.Hospedes.Count,
           src?.Hospedes.Select(c => ctx.Mapper.Map<SelecionarHospedesDto>(c)).ToImmutableList() ?? ImmutableList<SelecionarHospedesDto>.Empty
       ));
    }
}