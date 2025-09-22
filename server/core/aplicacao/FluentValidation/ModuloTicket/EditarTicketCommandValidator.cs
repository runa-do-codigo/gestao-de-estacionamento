using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloTicket;

public class EditarTicketCommandValidator : AbstractValidator<EditarTicketCommand>
{
    public EditarTicketCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do ticket é obrigatório.");
        RuleFor(x => x.DataEntrada)
            .NotEmpty().WithMessage("A data de entrada é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de entrada não pode ser futura.");
        RuleFor(x => x.DataSaida)
            .GreaterThan(x => x.DataEntrada).When(x => x.DataSaida.HasValue)
            .WithMessage("A data de saída deve ser maior que a data de entrada.");
        RuleFor(x => x.VagaId)
            .NotEmpty().WithMessage("A Vaga é obrigatório.");
    }
}