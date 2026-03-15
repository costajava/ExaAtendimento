using ExaAtendimento.Domain.Entities;

namespace ExaAtendimento.Application.DTOs
{
    public class AssuntoDto
    {
        public int Id { get; set; }
        public string TipoAssunto { get; set; } = string.Empty; 
        public int ModuloId { get; set; }
        public string? NomeModulo { get; set; }
    }
}
