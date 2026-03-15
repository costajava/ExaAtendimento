using FluentValidation;
using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Validators
{
    public class UsuarioRegistrarDtoValidator : AbstractValidator<UsuarioCriacaoDto>  
    {
        public UsuarioRegistrarDtoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("Nome do usuário é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Nome do usuário contém apenas espaços.")
                .WithMessage("Nome do usuário não pode conter apenas espaços.")
                .MaximumLength(50).WithMessage("Nome do usuário limitado a 50 caracteres.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email do usuário é obrigatório")
                .MaximumLength(100).WithMessage("Email do usuário limitado a 100 caracteres.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(p => p.Perfil)
                .IsInEnum().WithMessage("Perfil de usuário inválido.");

            RuleFor(p => p.Senha)
                .NotEmpty().WithMessage("Senha do usuário é obrigatório")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.")
                .MaximumLength(15).WithMessage("Senha limitado a 15 caracteres.");
                // melhorias sugeridas:
                //.Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
                //.Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula.")
                //.Matches(@"\d").WithMessage("Senha deve conter pelo menos um número.")
                //.Matches(@"[\W_]").WithMessage("Senha deve conter pelo menos um caractere especial.");

            RuleFor(p => p.ConfirmaSenha)
                .NotEmpty().WithMessage("Confirmação de senha é obrigatório")
                .Equal(p => p.Senha).WithMessage("Confirmação de senha incorreta");

        }
    }
}
