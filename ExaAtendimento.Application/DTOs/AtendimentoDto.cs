using ExaAtendimento.Domain.Enums;

namespace ExaAtendimento.Application.DTOs
{
    public class AtendimentoDto
    {
        public int Id { get; set; }

        public DateOnly DataAtendimento { get; set; }

        public DateTime DataRegistro { get; set; }

        public StatusAtendimento AtendimentoConcluido { get; set; }

        public string? Observacoes { get; set; }

        public bool CobrarCliente { get; set; }

        public TimeSpan HoraInicial { get; set; }

        public TimeSpan? HoraFinal { get; set; }

        public bool Encerrado { get; set; }

        public string Contato { get; set; } = string.Empty;

        public string? NumTipoAtendimento { get; set; }

        public int AnoRegistro { get; set; }

        public int? ClienteCodigo { get; set; }

        public int CaId { get; set; }
         
        public int? ClienteId { get; set; }

        public int SugestaoId { get; set; }

        public int ModuloId { get; set; }

        public int AssuntoId { get; set; }

        public int UsuarioId { get; set; }

        public int TipoAtendimentoId { get; set; }

        public string? NomeCa {  get; set; } 

        public string? NomeCliente {  get; set; }    

    }
}