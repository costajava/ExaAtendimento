using ExaAtendimento.Domain.Entities;
using ExaAtendimento.Domain.Enums;

namespace ExaAtendimento.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Atendente;
        public int? ModuloId { get; set; }
        public string? NomeModulo { get; set; }

    }
}
