using FluentValidation;
using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class CaDtoValidator : AbstractValidator<CaDto>
    {
        public CaDtoValidator() 
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Id da Ca é obrigatório");

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("Nome da Ca é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Nome da Ca contém apenas espaços.")
                .MaximumLength(100).WithMessage("Nome da Ca limitado a 100 caracteres.");

            RuleFor(p => p.Cidade)
                .NotEmpty().WithMessage("Cidade da Ca é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Cidade da Ca contém apenas espaços.")
                .MaximumLength(100).WithMessage("Cidade da Ca limitado a 100 caracteres.");

            RuleFor(p => p.Uf)
                .NotEmpty().WithMessage("UF da Ca é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("UF da Ca contém apenas espaços.")
                .Length(2).WithMessage("UF da Ca deve ter 2 caracteres.");

        }
    }
}
