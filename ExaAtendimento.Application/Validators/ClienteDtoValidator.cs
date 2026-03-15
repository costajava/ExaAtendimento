using FluentValidation;
using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class ClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        public ClienteDtoValidator() 
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Id do Cliente é obrigatório");

            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("Nome do Cliente é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Nome do cliente contém apenas espaços.")
                .MaximumLength(100).WithMessage("Nome do Cliente limitado a 100 caracteres.");

            RuleFor(p => p.Cidade)
                .NotEmpty().WithMessage("Cidade do Cliente é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Cidade do cliente contém apenas espaços.")
                .MaximumLength(100).WithMessage("Cidade do Cliente limitado a 100 caracteres.");

            RuleFor(p => p.Uf)
                .NotEmpty().WithMessage("UF do Cliente é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("UF do cliente contém apenas espaços.")
                .Length(2).WithMessage("UF do Cliente deve ter 2 caracteres.");

            RuleFor(p => p.CaId)
                .GreaterThan(0).WithMessage("Ca do Cliente é obrigatório");
        }
    }
}
