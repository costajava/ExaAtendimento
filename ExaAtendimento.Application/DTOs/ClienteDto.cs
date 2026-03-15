namespace ExaAtendimento.Application.DTOs
{
    public class ClienteDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Cidade { get; set; } = string.Empty;

        public string Uf { get; set; } = string.Empty;

        public int CaId { get; set; }

        public int? CaCompartilhadaId { get; set; }

        public string? NomeCa { get; set; }

        public string? NomeCaCompartilhada { get; set; }
    }
}
