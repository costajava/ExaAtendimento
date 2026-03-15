using ExaAtendimento.Application.DTOs;
using FluentValidation;

namespace ExaAtendimento.Application.Validators
{
    public class TipoAtendimentoDtoValidator : AbstractValidator<TipoAtendimentoDto>
    {
        public TipoAtendimentoDtoValidator()
        {
            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("Tipo de atendimento é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Tipo de atendimento contém apenas espaços.")
                .MaximumLength(50).WithMessage("Tipo de Atendimento limitado a 50 caracteres.");
        }
    }
}
