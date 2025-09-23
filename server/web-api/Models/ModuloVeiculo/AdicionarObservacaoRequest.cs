namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record AdicionarObservacaoRequest(string Observacao);

public record AdicionarObservacaoResponse(Guid Id, string Observacao);