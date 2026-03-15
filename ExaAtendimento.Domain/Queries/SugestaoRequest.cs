namespace ExaAtendimento.Domain.Queries
{
    public class SugestaoRequest : PageRequest
    {
        public string? Descricao { get; set; }
    }
}
