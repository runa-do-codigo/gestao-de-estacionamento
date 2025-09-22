using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloVaga.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloVaga;

public class EditarVagaCommandValidator : AbstractValidator<EditarVagaCommand>
{
    public EditarVagaCommandValidator()
    {
        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("O número da vaga é obrigatório.");
        RuleFor(x => x.Numero)
            .GreaterThan(0).WithMessage("O número da vaga deve ser maior que zero.");
        RuleFor(x => x.Numero)
            .LessThanOrEqualTo(1000).WithMessage("O número da vaga deve ser menor ou igual a 1000.");
        RuleFor(x => x.EstaOcupada)
            .NotNull().WithMessage("O campo 'EstaOcupada' é obrigatório.");
    }
}