using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloHospede.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloHospede;

public class EditarHospedeCommandValidator : AbstractValidator<EditarHospedeCommand>
{
    public EditarHospedeCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do hóspede é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do hóspede deve ter no máximo 100 caracteres.");
        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("O CPF do hóspede é obrigatório.")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$").WithMessage("O CPF do hóspede deve estar no formato XXX.XXX.XXX-XX.");
    }
}