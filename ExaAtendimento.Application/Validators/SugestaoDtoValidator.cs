using FluentValidation;

using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class SugestaoDtoValidator : AbstractValidator<SugestaoDto>
    {
        public SugestaoDtoValidator() 
        {
            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("Descrição da sugestão é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Descrição da sugestão contém apenas espaços.")
                .MaximumLength(50).WithMessage("Descrição da sugestão limitado a 50 caracteres.");
        }
    }
}
