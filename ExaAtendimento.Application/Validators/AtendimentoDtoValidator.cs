using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Domain.Enums;
using FluentValidation;
using Microsoft.Win32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExaAtendimento.Application.Validators
{
    public class AtendimentoDtoValidator : AbstractValidator<AtendimentoDto>
    {
        public AtendimentoDtoValidator()
        {
            RuleFor(p => p.DataAtendimento)
                .NotEqual(default(DateOnly)).WithMessage("Data de atendimento é obrigatória.");

            RuleFor(p => p.DataAtendimento)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("A data de atendimento não pode ser uma data futura.");

            RuleFor(p => p.DataRegistro)
                .NotEqual(default(DateTime)).WithMessage("Data de registro é obrigatória.");

            RuleFor(p => p.DataRegistro)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("A data de registro não pode ser uma data futura.");

            RuleFor(p => p.AtendimentoConcluido)
                .IsInEnum().WithMessage("Atendimento concluído inválido.");

            RuleFor(p => p.Observacoes)
                .MaximumLength(500).WithMessage("Observações limitado a 500 caracteres.");

            RuleFor(p => p.HoraInicial)
                .LessThanOrEqualTo(TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(59)).Add(TimeSpan.FromSeconds(59)))
                .WithMessage("Hora inicial inválida (limite 24h).");

            // Regras para HoraFinal quando atendimento for concluído
            When(p => p.AtendimentoConcluido == StatusAtendimento.Concluido, () =>
            {
                // REGRA 1: HoraFinal deve ser OBRIGATÓRIA (não nula)
                RuleFor(p => p.HoraFinal)
                    .NotNull()
                    .WithMessage("Hora final é obrigatória quando o atendimento está concluído.");

                // REGRA 2: Hora final deve ser maior ou igual à hora inicial
                RuleFor(p => p.HoraFinal)
                    .Must((dto, finalTime) => finalTime.HasValue && finalTime.Value >= dto.HoraInicial)
                    .WithMessage("Hora final não pode ser menor que a hora inicial.");

                // REGRA 3: Hora final deve ser válida (Limite 24h)
                RuleFor(p => p.HoraFinal)
                    .LessThanOrEqualTo(TimeSpan.FromDays(1).Subtract(TimeSpan.FromSeconds(1)))
                    .WithMessage("Hora final inválida (limite 24h).");
            });
            
            RuleFor(p => p.Contato)
                .NotEmpty().WithMessage("Contato é obrigatório")
                .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage("Contato contém apenas espaços.")
                .MaximumLength(50).WithMessage("Contato limitado a 50 caracteres.");

            RuleFor(p => p.AnoRegistro)
                .GreaterThan(0).WithMessage("Ano de registro é obrigatório");

            RuleFor(p => p.SugestaoId)
                .GreaterThan(0).WithMessage("Sugestão é obrigatório");

            RuleFor(p => p.ModuloId)
                .GreaterThan(0).WithMessage("Módulo é obrigatório");

            RuleFor(p => p.AssuntoId)
                .GreaterThan(0).WithMessage("Assunto é obrigatório");

            RuleFor(p => p.UsuarioId)
                .GreaterThan(0).WithMessage("Usuário é obrigatório");

            RuleFor(p => p.TipoAtendimentoId)
                .GreaterThan(0).WithMessage("Tipo de atendimento é obrigatório");
        }
    }
}
