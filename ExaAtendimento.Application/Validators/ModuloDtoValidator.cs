using FluentValidation;

using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class ModuloDtoValidator : AbstractValidator<ModuloDto>
    {
        public ModuloDtoValidator() 
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("Nome do módulo é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Nome do módulo contém apenas espaços.")
                .MaximumLength(50).WithMessage("Nome do módulo limitado a 50 caracteres.");
        }
    }
}
