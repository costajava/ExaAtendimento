using ExaAtendimento.Domain.Enums;

namespace ExaAtendimento.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Atendente;
    public string Senha { get; set; } = string.Empty;
    public string OldSenha { get; set; } = string.Empty;   
    public int? ModuloId { get; set; }
    public Modulo? Modulo { get; set; }
    public string? ResetSenhaToken { get; set; }
    public DateTime? ResetSenhaExpiracao { get; set; }

}
