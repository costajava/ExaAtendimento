namespace ExaAtendimento.Domain.Queries
{
    public class TipoAtendimentoRequest : PageRequest
    {
        public string? Descricao { get; set; }
    }
}
