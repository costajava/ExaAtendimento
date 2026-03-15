using FluentValidation;
using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class AssuntoDtoValidator : AbstractValidator<AssuntoDto>
    {
        public AssuntoDtoValidator() 
        {
            RuleFor(p => p.TipoAssunto)
                .NotEmpty().WithMessage("Tipo de assunto é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Tipo de assunto contém apenas espaços.")
                .MaximumLength(50).WithMessage("Tipo de assunto limitado a 50 caracteres.");

            RuleFor(p => p.ModuloId)
                .GreaterThan(0).WithMessage("Módulo é obrigatório");
        }
    }
}
