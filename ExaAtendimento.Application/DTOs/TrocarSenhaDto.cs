namespace ExaAtendimento.Application.DTOs
{
    public class TrocarSenhaDto
    {
        public int UsuarioId { get; set; }
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmaSenha { get; set; } = string.Empty;
    }
}
