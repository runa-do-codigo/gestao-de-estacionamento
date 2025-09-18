using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVeiculo.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloVeiculo;

public class CadastrarVeiculoValidator : AbstractValidator<CadastrarVeiculoCommand>
{
    public CadastrarVeiculoValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("A placa do veículo é obrigatória.")
            .Matches(@"^[A-Z]{3}-\d{4}$").WithMessage("A placa do veículo deve estar no formato ABC-1234.");
        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("O modelo do veículo é obrigatório.")
            .MaximumLength(50).WithMessage("O modelo do veículo deve ter no máximo 50 caracteres.");
        RuleFor(x => x.Cor)
            .NotEmpty().WithMessage("A cor do veículo é obrigatória.")
            .MaximumLength(30).WithMessage("A cor do veículo deve ter no máximo 30 caracteres.");
        RuleFor(x => x.HospedeId)
            .NotEmpty().WithMessage("O Hóspede é obrigatório.");
    }
}