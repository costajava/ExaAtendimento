namespace ExaAtendimento.Application.DTOs
{
    public class RedefinirSenhaDto
    {
        public string Email { get; set; } = string.Empty;
        public string CodigoSeguranca { get; set; } = string.Empty;   
        public string NovaSenha { get; set;} = string.Empty;
        public string ConfirmaSenha { get; set; } = string.Empty;
    }
}
