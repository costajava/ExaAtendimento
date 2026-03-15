using ExaAtendimento.Application.DTOs;

namespace ExaAtendimento.Application.Interfaces.Utils
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(EmailDto emailDto);
    }
}
