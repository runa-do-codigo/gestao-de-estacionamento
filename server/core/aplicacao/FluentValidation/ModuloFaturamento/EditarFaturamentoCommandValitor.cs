using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloFaturamento.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloFaturamento;

public class EditarFaturamentoCommandValitor : AbstractValidator<EditarFaturamentoCommand>
{
    public EditarFaturamentoCommandValitor()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do faturamento é obrigatório.");
        RuleFor(x => x.DataPagamento)
            .NotEmpty().WithMessage("A data de pagamento é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de pagamento não pode ser futura.");
        RuleFor(x => x.ValorTotal)
            .GreaterThan(0).WithMessage("O valor total deve ser maior que zero.");
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("O Ticket é obrigatório.");
    }
}