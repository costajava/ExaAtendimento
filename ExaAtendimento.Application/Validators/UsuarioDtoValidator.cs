using FluentValidation;
using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioDtoValidator() 
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("Nome do usuário é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Nome do usuário contém apenas espaços.")
                .MaximumLength(50).WithMessage("Nome do usuário limitado a 50 caracteres.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email do usuário é obrigatório")
                .MaximumLength(100).WithMessage("Email do usuário limitado a 100 caracteres.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(p => p.Perfil)
                .IsInEnum().WithMessage("Perfil de usuário inválido.");

        }
    }
}
