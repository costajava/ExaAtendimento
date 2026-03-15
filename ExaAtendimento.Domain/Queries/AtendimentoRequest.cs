using ExaAtendimento.Domain.Enums;

namespace ExaAtendimento.Domain.Queries
{
    public class AtendimentoRequest : PageRequest
    {
        public DateOnly? DataInicial { get; set; }
        public DateOnly? DataFinal { get; set; }
        public int UsuarioId { get; set; }
        public PerfilUsuario Perfil { get; set; }

    }
}
