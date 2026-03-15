using ExaAtendimento.Application.DTOs;
using ExaAtendimento.Application.Interfaces.Utils;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExaAtendimento.InfraData.Utils
{
    public class SmtpEmailServiceAsync : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailServiceAsync(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task EnviarEmailAsync(EmailDto emailDto)
        {
            // Carrega configurações (appsettings.json)
            var host = _configuration["EmailSettings:Host"];
            var porta = int.Parse(_configuration["EmailSettings:Porta"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

            var client = new SmtpClient(host, porta)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(username, "ControleAtendimento"),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = emailDto.IsBodyHtml
            };
            mailMessage.To.Add(emailDto.To);
         
            await client.SendMailAsync(mailMessage);
        }
    }
}
