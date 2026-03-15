namespace ExaAtendimento.Domain.Queries
{
    public class AssuntoRequest : PageRequest
    {
        public string? TipoAssunto { get; set; }
    }
}
