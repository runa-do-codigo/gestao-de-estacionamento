using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloVeiculo;

public class AdicionarObservacaoCommandValidator : AbstractValidator<AdicionarObservacaoCommand>
{
    public AdicionarObservacaoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do veículo é obrigatório.");

        RuleFor(x => x.Observacao)
            .NotEmpty().WithMessage("A observação é obrigatória.")
            .MaximumLength(500).WithMessage("A observação deve ter no máximo 500 caracteres.");
    }
}