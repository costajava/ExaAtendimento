
using ExaAtendimento.Domain.Enums;

namespace ExaAtendimento.Domain.Entities;

public class Atendimento
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

    public Ca? Ca { get; set; }

    public int? ClienteId { get; set; }

    public Cliente? Cliente { get; set; }   

    public int SugestaoId { get; set; }

    public Sugestao? Sugestao { get; set; }

    public int ModuloId { get; set; }

    public Modulo? Modulo { get; set; }

    public int AssuntoId { get; set; }

    public Assunto? Assunto { get; set; }

    public int UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }

    public int TipoAtendimentoId { get; set; } 

    public TipoAtendimento? TipoAtendimento { get; set; } 
}
