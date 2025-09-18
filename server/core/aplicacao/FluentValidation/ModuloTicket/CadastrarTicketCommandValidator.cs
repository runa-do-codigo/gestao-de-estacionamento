using FluentValidation;
using GestaoDeEstacionamento.Core.Aplicacao.ModuloTicket.Commands;

namespace GestaoDeEstacionamento.Core.Aplicacao.FluentValidation.ModuloTicket;

public class CadastrarTicketCommandValidator : AbstractValidator<CadastrarTicketCommand>
{
    public CadastrarTicketCommandValidator()
    {
        RuleFor(x => x.DataEntrada)
            .NotEmpty().WithMessage("A data de pagamento é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de pagamento não pode ser futura.");
        RuleFor(x => x.DataSaida)
           .GreaterThan(x => x.DataEntrada).When(x => x.DataSaida.HasValue)
           .WithMessage("A data de saída deve ser maior que a data de entrada.");
        RuleFor(x => x.VeiculoId)
            .NotEmpty().WithMessage("O Veiculo é obrigatório.");

        //RuleFor(x => x.VagaId)
            //.NotEmpty().WithMessage("A Vaga é obrigatório.");
    }
}